using AQMTemplate.Application.DTOs.Admin;

namespace AQMTemplate.Web.Extensions;

public class RequestModels
{
	public record GetUserByIdReques(string userId);
	public record UpdateUserRequest(string userId, UpdateUserDto updateUserDto);
	public record AddUserToRoleRequest(string userId, string roleId);
	public record RemoveUserFromRoleRequest(string userId, string roleId);

	public record GetRoleByIdRequest(string roleId);
	public record UpdateRoleRequest(string roleId, UpdateRoleDto updateRoleDto);
	public record AddPermissionToRoleRequest(string roleId, string permissionId);
	public record RemovePermissionFromRoleRequest(string roleId, string permissionId);

	public record GetPermissionByIdRequest(string permissionId);
	public record UpdatePermissionRequest(string permissionId, UpdatePermissionDto updatePermissionDto);

	public record GetMenuByIdRequest(string menuId);
	public record GetUserMenusRequest(string userId);
	public record UpdateMenuRequest(string menuId,  UpdateMenuDto updateMenuDto);
	public record GetChildMenusRequest(string parentMenuId);
	public record ToggleMenuStatusRequest(string menuId);

	public record ApiResponse<T>(bool success, T? Data, string? message, string[]? errors = null);

	public record PaginatedResponse<T>(
		IEnumerable<T> Data,
		int TotalCount,
		int PageNumber,
		int PageSize,
		bool HasNextPage,
		bool HasPreviousPage);

	public record PaginationRequest(int PageNumber = 1,  int PageSize = 10);
}
