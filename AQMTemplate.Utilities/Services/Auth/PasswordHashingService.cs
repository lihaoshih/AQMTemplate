using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Math.EC.Rfc8032;
using Utitlites.DTOs.Auth;


namespace Utitlites.Services.Auth;

public static class PasswordHashingService
{
    private const int SaltSizeBytes = 32;
    private const int HashSizeBytes = 64;
    private const int DefaultIterations = 600000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    private const char Delimiter = '$';

    public static HashedPasswordResult HashPassword(string password)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        
        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            saltBytes,
            DefaultIterations,
            Algorithm, 
            HashSizeBytes
        );

        return new HashedPasswordResult
        {
            SaltBase64 = Convert.ToBase64String(saltBytes),
            HashBase64 = Convert.ToBase64String(hashBytes)
        };
    }
    
    public static bool VerifyPassword(string passwordAttempt, string storedSaltBase64, string storedHashBase64)
    {
        if (string.IsNullOrWhiteSpace(storedSaltBase64) || string.IsNullOrWhiteSpace(storedHashBase64))
        {
            // 執行偽計算以避免時序攻擊
            PerformDummyComparison(passwordAttempt);
            return false;
        }

        try
        {
            byte[] saltBytes = Convert.FromBase64String(storedSaltBase64);
            byte[] storedHashBytes = Convert.FromBase64String(storedHashBase64);

            // 使用儲存的 Salt 和固定的迭代次數/演算法來計算輸入密碼的雜湊值
            byte[] computedHashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(passwordAttempt),
                saltBytes,
                DefaultIterations, // 必須與雜湊時使用的迭代次數相同
                Algorithm,  // 必須與雜湊時使用的演算法相同
                HashSizeBytes // 必須與雜湊時使用的雜湊大小相同
            );

            // 使用固定時間的比較來防止時序攻擊
            return CryptographicOperations.FixedTimeEquals(computedHashBytes, storedHashBytes);
        }
        catch (FormatException) // Base64 解碼失敗等
        {
            // 執行偽計算以避免時序攻擊
            PerformDummyComparison(passwordAttempt);
            return false;
        }
        catch (Exception) // 其他潛在錯誤
        {
            PerformDummyComparison(passwordAttempt);
            return false;
        }
    }
    
    private static void PerformDummyComparison(string password)
    {
        // 執行一次與正常驗證計算量相當的偽操作，以使錯誤路徑的執行時間與成功路徑相似
        byte[] dummySalt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), dummySalt, DefaultIterations, Algorithm, HashSizeBytes);
    }
    
    

}