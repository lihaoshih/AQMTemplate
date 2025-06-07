namespace AQMTemplate.Application.DTOs.Admin;

//public class PermissionDto
//{
//	public string PermissionId { get; set; } = string.Empty;
//	public string PermissionName { get; set; } = string.Empty;
//	public string? Description { get; set; }
//	public DateTime CreatedAt { get; set; }
//}
public record PermissionDto(
	string PermissionId,
	string PermissionName,
	string? Description,
	DateTime CreatedAt,
	DateTime ModifiedAt);

public record CreatePermissionDto(
	string PermissionName,
	string? Description);

public record UpdatePermissionDto(
	string PermissionName,
	string? Description);

public class CreatePermissionRequest
{
	public string PermissionName { get; set; } = string.Empty;
	public string? Description { get; set; }
}

public class UpdatePermissionRequest
{
	public string PermissionName { get; set; } = string.Empty;
	public string? Description { get; set; }
}