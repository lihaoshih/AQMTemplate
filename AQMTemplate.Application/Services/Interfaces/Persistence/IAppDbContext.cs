using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace AQMTemplate.Application.Services.Interfaces.Persistence
{
	public interface IAppDbContext
	{
		DbSet<Menu> Menus { get; set; }
		DbSet<Permission> Permissions { get; set; }
		DbSet<RolePermission> RolePermissions { get; set; }
		DbSet<Role> Roles { get; set; }
		DbSet<UserRole> UserRoles { get; set; }
		DbSet<User> Users { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}