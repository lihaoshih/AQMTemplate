using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Infrastructure.Persistence.Repositories.Admin
{
	public class PermissionRepository : IPermissionRepository
	{
		private readonly AppDbContext _appDbContext;

		public PermissionRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;			
		}

		public async Task<Permission?> GetByIdAsync(string permissionId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Permissions
				.Include(p => p.RolePermissions)
				.ThenInclude(rp => rp.Role)
				.FirstOrDefaultAsync(p => p.PermissionId == permissionId, cancellationToken);
		}

		public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Permissions 
				.Include(p => p.RolePermissions)
				.ThenInclude(rp => rp.Role) 
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Permission>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Permissions 
				.Include(p => p.RolePermissions) 
				.Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Permission>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Permissions
				.Include(p => p.RolePermissions)
				.ThenInclude(rp => rp.Role)
				.ThenInclude(r => r.UserRoles)
				.Where(p => p.RolePermissions.Any(rp => rp.Role.UserRoles.Any(ur => ur.UserId == userId)))
				.ToListAsync(cancellationToken);
		}

		public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
		{
			await _appDbContext.Permissions.AddAsync(permission, cancellationToken);
		}

		public void Remove(Permission permission)
		{
			_appDbContext.Permissions.Remove(permission);
		}

		public void Update(Permission permission)
		{
			_appDbContext.Permissions.Update(permission);
		}
	}
}
