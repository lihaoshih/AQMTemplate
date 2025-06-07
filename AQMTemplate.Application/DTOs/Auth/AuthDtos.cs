using System.ComponentModel.DataAnnotations;

namespace AQMTemplate.Application.DTOs.Auth;

public class LoginRequestDto
{
	[Required]
	public string Account { get; set; }
	[Required] 
	public string Password { get; set; }
}

public record LoginDto(
	string Account,
	string Password);

public record AuthResultDto(
	string UserId,
	string UserName,
	string? DisplayName,
	string Token,
	List<string> Roles,
	List<string> Permissions);

public record ChangePasswordDto(
	string UserId,
	string CurrentPassword,
	string NewPassword);

public class LoginResponseDto
{
	public string Token { get; set; } = null!;
	public string UserId { get; set; } = null!;
	public string Account { get; set; } = null!;
	public string Username { get; set; } = null!;
	public List<string> Roles { get; set; } = new List<string>();
}
