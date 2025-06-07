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
	public class RolePermissionRepository : IRolePermissionRepository
	{
		private readonly AppDbContext _appDbContext;

		public RolePermissionRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
		{
			await _appDbContext.RolePermissions.AddAsync(rolePermission, cancellationToken);
		}

		public async Task RemoveAsync(string roleId, string permissionId, CancellationToken cancellationToken = default)
		{
			var rolePermission = await _appDbContext.RolePermissions
				.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

			if (rolePermission != null)
			{
				_appDbContext.RolePermissions.Remove(rolePermission);
			}
		}
	}
}
