namespace Utilities.DTOs.Security;

public class CryptographyResponse
{
    public string Result { get; set; } = null!;
    public string Algorithm { get; set; } = null!;
    public string BlockCipherMode { get; set; } = null!;
    public string PaddingMode { get; set; } = null!;
    public string? AsymmetricKey { get; set; }
}