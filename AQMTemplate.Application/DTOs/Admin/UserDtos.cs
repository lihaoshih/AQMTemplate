namespace AQMTemplate.Application.DTOs.Admin;

// For Reading User data
//public class UserDto
//{
//	public string UserId { get; set; } = string.Empty;
//	public string Account { get; set; } = string.Empty;
//	public string Username { get; set; } = string.Empty;
//	public string? DisplayName { get; set; }
//	public string? Department { get; set; }
//	public string? JobTitle { get; set; }
//	public string? Email { get; set; }
//	public string? Phone { get; set; }
//	public bool IsActive { get; set; }
//	public bool IsLocked { get; set; }
//	public DateTime CreatedAt { get; set; }
//	public DateTime? LastLogin { get; set; }
//	public List<string> RoleIds { get; set; } = new(); // Assigned role IDs
//	public List<RoleDto> Roles { get; set; } = new(); // Assigned role details (optional)
//}

public record UserDto(
	string UserId,
	string Account,
	string? Department,
	string? JobTitle,
	string UserName,
	string? DisplayName,
	string? AvatarUrl,
	string? Alias,
	char Gender,
	string? IdNumber,
	DateOnly? Birthday,
	string? PostalCode,
	string? MailingAddress,
	string? Email,
	string? Phone,
	bool IsActive,
	bool IsLocked,
	int FailedLoginAttempt,
	DateTime? LastLogin,
	string? Annotation,
	DateTime CreatedAt,
	DateTime ModifiedAt);

public record CreateUserDto(
	string Account,
	string Password,
	string? Department,
	string? JobTitle,
	string UserName,
	string? DisplayName,
	string? Alias,
	char Gender,
	string? IdNumber,
	DateOnly? Birthday,
	string? PostalCode,
	string? MailingAddress,
	string? Email,
	string? Phone,
	string? Annotation);

public record UpdateUserDto(
	string? Department,
	string? JobTitle,
	string? DisplayName,
	string? AvatarUrl,
	string? Alias,
	string? IdNumber,
	DateOnly? Birthday,
	string? PostalCode,
	string? MailingAddress,
	string? Email,
	string? Phone, 
	string? Annotation);

// For Creating a new User
public class CreateUserRequest
{
	public string Account { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty; // Plain text password, to be hashed by the service
	public string Username { get; set; } = string.Empty;
	public string? DisplayName { get; set; }
	public char Gender { get; set; } // Make sure to validate (e.g., 'M', 'F', 'O')
	public string? Department { get; set; }
	public string? JobTitle { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public bool IsActive { get; set; } = true;
	public List<string> RoleIds { get; set; } = new(); // RoleIds to assign
}

// For Updating an existing User
public class UpdateUserRequest
{
	// UserId will come from the route or another source, not usually in the body for PUT
	public string Account { get; set; } = string.Empty; // Usually not updatable, or requires special permissions
	public string Username { get; set; } = string.Empty;
	public string? DisplayName { get; set; }
	public char Gender { get; set; }
	public string? Department { get; set; }
	public string? JobTitle { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public bool IsActive { get; set; }
	public bool IsLocked { get; set; } // Allow locking/unlocking
	public List<string> RoleIds { get; set; } = new(); // Full list of RoleIds to be set
}

// For changing user's password
public class ChangeUserPasswordRequest
{
	public string NewPassword { get; set; } = string.Empty;
}

// For assigning roles to user
public class AssignRolesToUserRequest
{
	public List<string> RoleIds { get; set; } = new();
}