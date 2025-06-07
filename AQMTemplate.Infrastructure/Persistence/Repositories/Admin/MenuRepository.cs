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
	public class MenuRepository : IMenuRepository
	{
		private readonly AppDbContext _appDbContext;

		public MenuRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<Menu?> GetByIdAsync(string menuId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Menus
				.Include(m => m.ParentMenu)
				.Include(m => m.ChildMenus)
				.Include(m => m.Permission)
				.FirstOrDefaultAsync(m => m.MenuId == menuId, cancellationToken);
		}

		public async Task<IEnumerable<Menu>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var firstUser = await _appDbContext.Users.FirstOrDefaultAsync();
				if (firstUser != null)
				{
					Console.WriteLine($"Successfully connected and fetch user: {firstUser}");
				}
				else
				{
					Console.WriteLine("Connected, but no users found");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}


			return await _appDbContext.Menus
				.Include(m => m.ParentMenu)
				.Include(m => m.Permission)
				.OrderBy(m => m.SortOrder)
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Menu>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			// 獲取用戶的所有權限
			var permissions = await _appDbContext.Permissions
				.Include(p => p.RolePermissions)
				.ThenInclude(rp => rp.Role)
				.ThenInclude(r => r.UserRoles)
				.Where(p => p.RolePermissions.Any(rp => rp.Role.UserRoles.Any(ur => ur.UserId == userId)))
				.Select(p => p.PermissionId)
				.ToListAsync(cancellationToken);

			// 獲取用戶有權訪問的選單
			return await _appDbContext.Menus
				.Include(m => m.ParentMenu)
				.Include(m => m.Permission)
				.Where(m => m.IsVisible && (m.PermissionId == null || permissions.Contains(m.PermissionId)))
				.OrderBy(m => m.SortOrder)
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Menu>> GetRootMenusAsync(CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Menus
				.Include(m => m.Permission)
				.Where(m => m.ParentMenuId == null && m.IsVisible)
				.OrderBy(m => m.SortOrder)
				.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<Menu>> GetChildMenusAsync(string parentMenuId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Menus
				.Include(m => m.Permission)
				.Where(m => m.ParentMenuId == parentMenuId && m.IsVisible)
				.OrderBy(m => m.SortOrder)
				.ToListAsync(cancellationToken);
		}

		public async Task AddAsync(Menu menu, CancellationToken cancellationToken = default)
		{
			await _appDbContext.Menus.AddAsync(menu, cancellationToken);
		}

		public void Update(Menu menu)
		{
			_appDbContext.Menus.Update(menu);
		}

		public void Remove(Menu menu)
		{
			_appDbContext.Menus.Remove(menu);
		}
	}
}
