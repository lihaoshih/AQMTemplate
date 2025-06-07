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
	public class RoleRepository : IRoleRepository
	{
		private readonly AppDbContext _appDbContext;

		public RoleRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;			
		}

		public async Task<Role?> GetByIdAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
		}

		public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Role>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
				.ToListAsync(cancellationToken);
		}

		public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
		{
			await _appDbContext.Roles.AddAsync(role, cancellationToken);
		}

		public void Update(Role role)
		{
			_appDbContext.Roles.Update(role);
		}

		public void Remove(Role role)
		{
			_appDbContext.Roles.Remove(role);
		}

	}
}
