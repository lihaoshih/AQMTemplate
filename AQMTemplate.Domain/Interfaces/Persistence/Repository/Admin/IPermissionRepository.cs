using AQMTemplate.Domain.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IPermissionRepository
	{
		Task<Permission?> GetByIdAsync(string permissionId, CancellationToken cancellationToken = default);
		Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<Permission>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);
		Task<IEnumerable<Permission>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
		Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
		void Update(Permission permission);
		void Remove(Permission permission);
	}
}
