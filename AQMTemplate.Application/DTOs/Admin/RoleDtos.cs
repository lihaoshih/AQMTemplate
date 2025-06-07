namespace AQMTemplate.Application.DTOs.Admin;

//public class RoleDto
//{
//	public string RoleId { get; set; } = string.Empty;
//	public string RoleName { get; set; } = string.Empty;
//	public string? Description { get; set; }
//	public DateTime CreatedAt { get; set; }
//	public List<string> PermissionIds { get; set; } = new(); // Assigned permission IDs
//}

public record RoleDto(
	string RoleId,
	string RoleName,
	string? Description,
	DateTime CreatedAt,
	DateTime ModifiedAt);

public record CreateRoleDto(
	string RoleName,
	string? Description);

public record UpdateRoleDto(
	string RoleName,
	string? Description);

public class CreateRoleRequest
{
	public string RoleName { get; set; } = string.Empty;
	public string? Description { get; set; }
	public List<string> PermissionIds { get; set; } = new();
}

public class UpdateRoleRequest
{
	public string RoleName { get; set; } = string.Empty;
	public string? Description { get; set; }
	public List<string> PermissionIds { get; set; } = new();
}

public class AssignPermissionsToRoleRequest
{
	public List<string> PermissionIds { get; set; } = new();
}