using AQMTemplate.Domain.Entities.Admin;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IRolePermissionRepository
	{
		Task AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
		Task RemoveAsync(string roleId, string permissionId,  CancellationToken cancellationToken = default);
	}
}
