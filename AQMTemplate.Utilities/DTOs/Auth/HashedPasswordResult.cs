namespace Utitlites.DTOs.Auth;

public class HashedPasswordResult
{
    public string SaltBase64 { get; set; }
    public string HashBase64 { get; set; }
}