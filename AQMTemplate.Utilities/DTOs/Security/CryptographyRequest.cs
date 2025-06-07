using AQMTemplate.Domain.Enums.Security;

namespace Utilities.DTOs.Security;

public class CryptographyRequest
{
    public CryptoOperation Operation { get; set; }
    public string InputText { get; set; } = null!;
    public string PassPhrase { get; set; } = null!;
    public CryptoAlgorithm Algorithm { get; set; }
    public CipherModeEnum CipherMode { get; set; }
    public PaddingModeEnum PaddingMode { get; set; }
    public string? AsymmetricKey { get; set; }
}