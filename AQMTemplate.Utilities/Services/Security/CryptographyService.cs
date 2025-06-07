using System.Security.Cryptography;
using System.Text;
using AQMTemplate.Domain.Enums;
using AQMTemplate.Domain.Enums.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Utilities.DTOs.Security;

namespace Utilities.Services.Security;

public static class CryptographyService
{
    private const int Pbkdf2Iterations = 210_000;
    
    // AES-GCM 相關常數
    private const int AesGcmSaltSize = 16;
    private const int AesGcmNonceSize = 12;
    private const int AesGcmTagSize = 16;
    private const int AesKeySizeInBits = 256;
    
    // 非 AEAD 對稱加密的 Salt 大小
    private const int NonAeadSaltSize = 16;

    public static CryptographyResponse Crypto(CryptographyRequest request)
    {
        if (request.Algorithm.ToString() == "RSA")
        {
            return HandleAsymmetric(request);
        }
        else
        {
            return HandleSymmetric(request);
        }
    }

    public static HashingResponse Hash(HashingRequest request)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(request.InputText);
        byte[] hash = request.Algorithm switch
        {
            HashingAlgorithm.MD5 => MD5.Create().ComputeHash(inputBytes),
            HashingAlgorithm.SHA1 => SHA1.Create().ComputeHash(inputBytes),
            HashingAlgorithm.SHA256 => SHA256.Create().ComputeHash(inputBytes),
            HashingAlgorithm.SHA384 => SHA384.Create().ComputeHash(inputBytes),
            HashingAlgorithm.SHA512 => SHA512.Create().ComputeHash(inputBytes),
            _ => throw new NotSupportedException($"不支援的雜湊演算法: {request.Algorithm}")
        };

        return new HashingResponse
        {
            Result = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpperInvariant(),
            Algorithm = request.Algorithm.ToString().ToUpperInvariant()
        };
    }
  
    private static CryptographyResponse HandleSymmetric(CryptographyRequest request)
    {
        bool isEncrypt = request.Operation == CryptoOperation.Encrypt;

        if (request.Algorithm == CryptoAlgorithm.AES && request.CipherMode == CipherModeEnum.GCM)
        {
            byte[] key;
            byte[] outputPayload;

            if (isEncrypt) // AEAD Encrypt
            {
                byte[] saltBytes = RandomNumberGenerator.GetBytes(AesGcmSaltSize);
                key = DeriveKeyFromPassPhrase(request.PassPhrase, saltBytes, AesKeySizeInBits, Pbkdf2Iterations);
                
                byte[] nonceBytes = RandomNumberGenerator.GetBytes(AesGcmNonceSize);
                byte[] plainBytes = Encoding.UTF8.GetBytes(request.InputText);
                byte[] cipherBytes = new byte[plainBytes.Length];
                byte[] tagBytes = new byte[AesGcmTagSize];

                using (var aesGcm = new AesGcm(key))
                {
                    aesGcm.Encrypt(nonceBytes, plainBytes, cipherBytes, tagBytes);
                }
                
                outputPayload = new byte[saltBytes.Length + nonceBytes.Length + cipherBytes.Length + tagBytes.Length];
				Buffer.BlockCopy(saltBytes, 0, outputPayload, 0, saltBytes.Length);
				Buffer.BlockCopy(nonceBytes, 0, outputPayload, saltBytes.Length, nonceBytes.Length);
				Buffer.BlockCopy(cipherBytes, 0, outputPayload, saltBytes.Length + nonceBytes.Length, cipherBytes.Length);
				Buffer.BlockCopy(tagBytes, 0, outputPayload, saltBytes.Length + nonceBytes.Length + cipherBytes.Length, tagBytes.Length);
				//Buffer.BlockCopy(saltBytes, 0, outputPayload, 0, saltBytes.Length);
				//Buffer.BlockCopy(nonceBytes, 0, outputPayload, saltBytes.Length, nonceBytes.Length);
				//Buffer.BlockCopy(cipherBytes, 0, outputPayload, saltBytes.Length + nonceBytes.Length, nonceBytes.Length);
				//Buffer.BlockCopy(tagBytes, 0, outputPayload, saltBytes.Length + nonceBytes.Length + cipherBytes.Length, tagBytes.Length);
			}
            else // AEAD Decrypt
            {
                byte[] inputBytesFromBase64 = Convert.FromBase64String(request.InputText);

                if (inputBytesFromBase64.Length < AesGcmSaltSize + AesGcmNonceSize + AesGcmTagSize)
                {
                    throw new ArgumentException("輸入字串過短，無法包含 AES-GCM 所需的 Salt, Nonce, Ciphertext 和 Tag。");
                }

                ReadOnlySpan<byte> inputSpan = inputBytesFromBase64.AsSpan();
                ReadOnlySpan<byte> saltBytes = inputSpan.Slice(0, AesGcmSaltSize);
                key = DeriveKeyFromPassPhrase(request.PassPhrase, saltBytes.ToArray(), AesKeySizeInBits,
                    Pbkdf2Iterations);

                ReadOnlySpan<byte> nonceBytes = inputSpan.Slice(AesGcmSaltSize, AesGcmNonceSize);
                ReadOnlySpan<byte> tagBytes =
                    inputSpan.Slice(inputBytesFromBase64.Length - AesGcmTagSize, AesGcmTagSize);

                int ciphertextSize = inputBytesFromBase64.Length - AesGcmSaltSize - AesGcmNonceSize - AesGcmTagSize;
                if (ciphertextSize < 0) throw new ArgumentException("無效的 AES-GCM 密文結構。");
                ReadOnlySpan<byte> cipherBytes = inputSpan.Slice(AesGcmSaltSize + AesGcmNonceSize, ciphertextSize);

                outputPayload = new byte[ciphertextSize]; // 這將儲存解密後的明文

                try
                {
                    using (var aesGcm = new AesGcm(key))
                    {
                        aesGcm.Decrypt(nonceBytes, cipherBytes, tagBytes, outputPayload);
                    }
                }
                catch (CryptographicException ex)
                {
                    throw new InvalidOperationException("AES-GCM 解密失敗。資料可能已損毀、遭篡改或密碼不正確。", ex);
                }
            }

            return new CryptographyResponse
            {
                Result = isEncrypt ? Convert.ToBase64String(outputPayload) : Encoding.UTF8.GetString(outputPayload),
                Algorithm = request.Algorithm.ToString().ToUpperInvariant(),
                BlockCipherMode = "GCM",
                PaddingMode = "None"
            };
        } else
        {
            byte[] currentSaltBytes;
            byte[] derivedKey;
            byte[] ivBytes;
            byte[] processedBytes; // 加密或解密後的位元組

            using SymmetricAlgorithm alg = MapSymmetricAlgorithm(request.Algorithm);

            if (request.CipherMode == CipherModeEnum.ECB)
            {
                // 考慮記錄警告或直接拋出例外
                //System.Diagnostics.Debug.WriteLine("警告: ECB 模式極不安全，不應使用。");
                throw new NotSupportedException("ECB mode is insecure and not recommended.");
            }
            
            alg.Mode = MapCipherMode(request.CipherMode); // 此處 CipherModeEnum.GCM 會導致例外，但已被上方 if 攔截
            alg.Padding = MapPaddingMode(request.PaddingMode);
            
            int keySizeInBits = alg.KeySize;
            int blockSizeInBytes = alg.BlockSize / 8; // IV 大小通常等於區塊大小

            if (isEncrypt)
            {
                currentSaltBytes = RandomNumberGenerator.GetBytes(NonAeadSaltSize);
                derivedKey = DeriveKeyFromPassPhrase(request.PassPhrase, currentSaltBytes, keySizeInBits, Pbkdf2Iterations);
                ivBytes = RandomNumberGenerator.GetBytes(blockSizeInBytes);

                alg.Key = derivedKey;
                alg.IV = ivBytes;

                byte[] plainBytes = Encoding.UTF8.GetBytes(request.InputText);
                using ICryptoTransform transform = alg.CreateEncryptor();
                byte[] encryptedBytes = transform.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                processedBytes = new byte[currentSaltBytes.Length + ivBytes.Length + encryptedBytes.Length];
                Buffer.BlockCopy(currentSaltBytes, 0, processedBytes, 0, currentSaltBytes.Length);
                Buffer.BlockCopy(ivBytes, 0, processedBytes, currentSaltBytes.Length, ivBytes.Length);
                Buffer.BlockCopy(encryptedBytes, 0, processedBytes, currentSaltBytes.Length + ivBytes.Length, encryptedBytes.Length);
            }
            else // Decrypt (non-AEAD)
            {
                byte[] inputBytesFromBase64 = Convert.FromBase64String(request.InputText);

                if (inputBytesFromBase64.Length < NonAeadSaltSize + blockSizeInBytes)
                {
                    throw new ArgumentException("輸入字串過短，無法包含 Salt, IV 和 Ciphertext。");
                }
                
                ReadOnlySpan<byte> inputSpan = inputBytesFromBase64.AsSpan();
                currentSaltBytes = inputSpan.Slice(0, NonAeadSaltSize).ToArray();
                ivBytes = inputSpan.Slice(NonAeadSaltSize, blockSizeInBytes).ToArray();
                
                derivedKey = DeriveKeyFromPassPhrase(request.PassPhrase, currentSaltBytes, keySizeInBits, Pbkdf2Iterations);
                
                alg.Key = derivedKey;
                alg.IV = ivBytes;

                int ciphertextSize = inputBytesFromBase64.Length - NonAeadSaltSize - blockSizeInBytes;
                if (ciphertextSize < 0) throw new ArgumentException("無效的密文結構。");
                byte[] cipherBytes = inputSpan.Slice(NonAeadSaltSize + blockSizeInBytes, ciphertextSize).ToArray();

                try
                {
                    using ICryptoTransform transform = alg.CreateDecryptor();
                    processedBytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                }
                catch (CryptographicException ex)
                {
                    throw new InvalidOperationException("解密失敗。資料可能已損毀、遭篡改或密碼不正確。", ex);
                }
            }

            return new CryptographyResponse
            {
                Result = isEncrypt ? Convert.ToBase64String(processedBytes) : Encoding.UTF8.GetString(processedBytes),
                Algorithm = request.Algorithm.ToString(),
                BlockCipherMode = request.CipherMode.ToString(),
                PaddingMode = request.PaddingMode.ToString()
            };
        }
    }
    
    private static CryptographyResponse HandleAsymmetric(CryptographyRequest request)
    {
        bool isEncrypt = request.Operation == CryptoOperation.Encrypt;

        if (isEncrypt)
        {
            // 從 PassPhrase 派生金鑰對 (確定性，相同 PassPhrase 產生相同金鑰對)
            var (publicXml, privateXml) = DeriveRsaKeyPair(request.PassPhrase);
            var inputBytes = Encoding.UTF8.GetBytes(request.InputText);
            
            using var rsa = RSA.Create();
            rsa.FromXmlString(publicXml); // 使用公鑰加密
            var cipheredText = rsa.Encrypt(inputBytes, RSAEncryptionPadding.OaepSHA512); // 建議 OAEP SHA256 或更高

            return new CryptographyResponse
            {
                Result = Convert.ToBase64String(cipheredText),
                Algorithm = $"{request.Algorithm.ToString()}",
                AsymmetricKey = privateXml // 返回私鑰給呼叫者儲存，以便後續解密
            };
        }
        else // Decrypt
        {
            if (string.IsNullOrWhiteSpace(request.AsymmetricKey))
                throw new ArgumentException("解密時 RSA 私鑰不可為空。");

            var cipherBytes = Convert.FromBase64String(request.InputText);
            using var rsa = RSA.Create();
            rsa.FromXmlString(request.AsymmetricKey); // 使用私鑰解密
            var plain = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA512);

            return new CryptographyResponse()
            {
                Result = Encoding.UTF8.GetString(plain),
                Algorithm = $"{request.Algorithm.ToString()}"
            };
        }
    }

    private static byte[] DeriveKeyFromPassPhrase(string passPhrase, byte[] saltBytes, int keySizeInBits, int iterations)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(passPhrase),
            saltBytes,
            iterations,
            HashAlgorithmName.SHA512,
            keySizeInBits / 8
        );
    }

    private static SymmetricAlgorithm MapSymmetricAlgorithm(CryptoAlgorithm algorithm) => algorithm switch
    {
        CryptoAlgorithm.AES => Aes.Create(),
        CryptoAlgorithm.TripleDES => TripleDES.Create(),
        _ => throw new NotSupportedException($"對稱式加密演算法部支援: {algorithm.ToString()}")
    };

    private static CipherMode MapCipherMode(CipherModeEnum mode) => mode switch
    {
        CipherModeEnum.CBC => CipherMode.CBC,
        CipherModeEnum.ECB => CipherMode.ECB,
        CipherModeEnum.CFB => CipherMode.CFB,
        CipherModeEnum.OFB => CipherMode.OFB,
        CipherModeEnum.GCM => throw new InvalidOperationException("GCM 模式應獨立處理，不透過此方法映射。"), // GCM 被特別處理
        _ => throw new NotSupportedException($"不支援的區塊加密模式：{mode}")
    };

    private static PaddingMode MapPaddingMode(PaddingModeEnum mode) => mode switch
    {
        PaddingModeEnum.PKCS7 => PaddingMode.PKCS7,
        PaddingModeEnum.Zeros => PaddingMode.Zeros,
        PaddingModeEnum.ISO10126 => PaddingMode.ISO10126,
        PaddingModeEnum.ANSIX923 => PaddingMode.ANSIX923,
        _ => throw new NotSupportedException($"不支援的填充模式: {mode.ToString()}")
    };

    /*
    /*
    private int GetBlockSizeInBytes(CryptoAlgorithm algorithm)
    {
        switch (algorithm)
        {
            case CryptoAlgorithm.AES:
                return 16;
            case CryptoAlgorithm.DES:
            case CryptoAlgorithm.TripleDES:
            case CryptoAlgorithm.RC2:
                return 8;
            default:
                throw new ArgumentOutOfRangeException(nameof(algorithm), "不支援的演算法");
            
        }
    }
    #1#

    private void DeriveKeyIV(
        string passPhrase,
        string salt,
        int keySize,
        int blockSize,
        out byte[] derivedKey,
        out byte[] derivedIv)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        using var deriveBytes = new Rfc2898DeriveBytes(passPhrase, saltBytes, 10_000);
        derivedKey = deriveBytes.GetBytes(keySize);
        derivedIv = deriveBytes.GetBytes(blockSize);
    }*/

    private static (string publicXml, string privateXml) DeriveRsaKeyPair(string passPhrase)
    {
        var seed = SHA512.HashData(Encoding.UTF8.GetBytes(passPhrase));
        
        var digestGen = new DigestRandomGenerator(new Org.BouncyCastle.Crypto.Digests.Sha512Digest());
        digestGen.AddSeedMaterial(seed);
        var secureRnd = new SecureRandom(digestGen);

        var keygen = new RsaKeyPairGenerator();
        keygen.Init(new KeyGenerationParameters(secureRnd, 4096));
        AsymmetricCipherKeyPair keyPair = keygen.GenerateKeyPair();
        
        var pubParam = (RsaKeyParameters)keyPair.Public;
        var privParam = (RsaPrivateCrtKeyParameters)keyPair.Private;
        
        string publicXml, privateXml;
        using (var rsaPub = RSA.Create())
        {
            rsaPub.ImportParameters(DotNetUtilities.ToRSAParameters(pubParam));
            publicXml = rsaPub.ToXmlString(false);
        }

        using (var rsaPriv = RSA.Create())
        {
            rsaPriv.ImportParameters(DotNetUtilities.ToRSAParameters(privParam));
            privateXml = rsaPriv.ToXmlString(true);
        }

        return (publicXml, privateXml);
    }
}