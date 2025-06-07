using System.ComponentModel.DataAnnotations;

namespace Utitlites.DTOs.Auth;

public class VerifyPasswordRequest
{
    [Required]
    public string Account { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}