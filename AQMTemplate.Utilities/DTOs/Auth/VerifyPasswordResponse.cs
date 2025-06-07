namespace Utitlites.DTOs.Auth;

public class VerifyPasswordResponse
{
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }
    public string? RetrievedSalt { get; set; }
    public string? RetrivedHash { get; set; }
}