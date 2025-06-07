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
	public class UserRoleRepository : IUserRoleRepository
	{
		private readonly AppDbContext _appDbContext;

		public UserRoleRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
		{
			await _appDbContext.UserRoles.AddAsync(userRole, cancellationToken);
		}

		public async Task RemoveAsync(string userId, string roleId, CancellationToken cancellationToken = default)
		{
			var userRole = await _appDbContext.UserRoles
				.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

			if (userRole != null)
			{
				_appDbContext.UserRoles.Remove(userRole);
			}
		}
	}
}
