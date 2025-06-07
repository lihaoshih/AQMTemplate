using AQMTemplate.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Application.Services.Interfaces.Admin;

public interface IPermissionService
{
	Task<PermissionDto> GetPermissionByIdAsync(string permissionId, CancellationToken cancellationToken = default);
	Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
	Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto, CancellationToken cancellationToken = default);
	Task<PermissionDto> UpdatePermissionAsync(string permissionId, UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken = default);
	Task<bool> DeletePermissionAsync(string permissionId, CancellationToken cancellationToken = default);
	Task<IEnumerable<RoleDto>> GetPermissionRolesAsync(string permissionId, CancellationToken cancellationToken = default);
}
