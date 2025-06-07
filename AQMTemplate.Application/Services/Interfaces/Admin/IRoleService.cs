using AQMTemplate.Application.DTOs.Admin;

namespace AQMTemplate.Application.Services.Interfaces.Admin;

public interface IRoleService
{
	Task<RoleDto> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
	Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
	Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto, CancellationToken cancellationToken = default);
	Task<RoleDto> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto, CancellationToken cancellationToken = default);
	Task<bool> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default);
	Task<bool> AddPermissionToRoleAsync(string roleId, string permissionId, CancellationToken cancellationToken = default);
	Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionId, CancellationToken cancellationToken = default);
	Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default);
	Task<IEnumerable<UserDto>> GetRoleUsersAsync(string roleId, CancellationToken cancellationToken = default);
}
