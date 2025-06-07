using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Admin;
using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Domain.Exceptions;
using AQMTemplate.Domain.Exceptions.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;

namespace AQMTemplate.Application.Services.Implementations.Admin;

public class MenuService : IMenuService
{ 
	private readonly IMenuRepository _menuRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IUnitOfWork _unitOfWork;

	public MenuService(
		IMenuRepository menuRepository, 
		IPermissionRepository permissionRepository, 
		IUnitOfWork unitOfWork)
	{
		_menuRepository = menuRepository;
		_permissionRepository = permissionRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<MenuDto> GetMenuByIdAsync(string menuId, CancellationToken cancellationToken = default)
	{
		var menu = await _menuRepository.GetByIdAsync(menuId, cancellationToken)
			?? throw new DomainException($"{menuId}找不到");

		return MapToMenuDto(menu);
	}

	public async Task<IEnumerable<MenuDto>> GetAllMenusAsync(CancellationToken cancellationToken = default)
	{
		var menus = await _menuRepository.GetAllAsync(cancellationToken);
		return menus.Select(MapToMenuDto);
	}

	public async Task<IEnumerable<MenuDto>> GetRootMenusAsync(CancellationToken cancellationToken = default)
	{
		var rootMenus = await _menuRepository.GetRootMenusAsync(cancellationToken);
		var result = new List<MenuDto>();

		foreach (var menu in rootMenus)
		{
			var menuDto = MapToMenuDto(menu);
			var childMenus = await _menuRepository.GetChildMenusAsync(menu.MenuId, cancellationToken);

			if (childMenus.Any())
			{
				menuDto = menuDto with { Children = childMenus.Select(MapToMenuDto).ToList() };
			}

			result.Add(menuDto);
		}

		return result;
	}

	public async Task<IEnumerable<MenuDto>> GetChildMenusAsync(string parentMenuId, CancellationToken cancellationToken = default)
	{
		var childMenus = await _menuRepository.GetChildMenusAsync(parentMenuId, cancellationToken);
		return childMenus.Select(MapToMenuDto);
	}

	public async Task<IEnumerable<MenuDto>> GetUserMenusAsync(string userId, CancellationToken cancellationToken = default)
	{
		var userMenus = await _menuRepository.GetByUserIdAsync(userId, cancellationToken);
		var rootMenus = userMenus.Where(m => m.ParentMenuId == null).OrderBy(m => m.SortOrder).ToList();
		var result = new List<MenuDto>();

		foreach (var menu in rootMenus)
		{
			var menuDto = MapToMenuDto(menu);
			var childMenus = userMenus
				.Where(m => m.ParentMenuId == menu.MenuId)
				.OrderBy(m => m.SortOrder)
				.ToList();

			if (childMenus.Any())
			{
				menuDto = menuDto with { Children = childMenus.Select(MapToMenuDto).ToList() };
			}

			result.Add(menuDto);
		}

		return result;
	}

	public async Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default)
	{
		if (!string.IsNullOrEmpty(createMenuDto.ParentMenuId))
		{
			var parentMenu = await _menuRepository.GetByIdAsync(createMenuDto.ParentMenuId, cancellationToken);
			if (parentMenu == null)
			{
				throw new DomainException($"{createMenuDto.ParentMenuId}父層功能選單不存在！");
			}
		}

		if (!string.IsNullOrEmpty(createMenuDto.PermissionId))
		{
			var permission = await _permissionRepository.GetByIdAsync(createMenuDto.PermissionId, cancellationToken);
			if (permission == null)
			{
				throw new PermissionNotFoundException(createMenuDto.PermissionId);
			}
		}

		var menu = new Menu(
			createMenuDto.MenuName,
			createMenuDto.ParentMenuId,
			createMenuDto.Icon,
			createMenuDto.Url,
			createMenuDto.PermissionId,
			createMenuDto.SortOrder,
			createMenuDto.IsVisible);

		await _menuRepository.AddAsync(menu, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return MapToMenuDto(menu);
	}

	public async Task<MenuDto> UpdateMenuAsync(string menuId, UpdateMenuDto updateMenuDto, CancellationToken cancellationToken = default)
	{
		var menu = await _menuRepository.GetByIdAsync(menuId, cancellationToken)
			?? throw new DomainException($"{menuId}功能選單不存在！");

		if (!string.IsNullOrEmpty(updateMenuDto.ParentMenuId))
		{
			var parentMenu = await _menuRepository.GetByIdAsync(updateMenuDto.ParentMenuId, cancellationToken);
			if (parentMenu == null)
			{
				throw new DomainException($"{updateMenuDto.ParentMenuId}父層功能選單不存在！");
			}

			if (menuId == updateMenuDto.ParentMenuId)
			{
				throw new DomainException($"子選單不能是父層功能選單！");
			}
		}

		if (!string.IsNullOrEmpty(updateMenuDto.PermissionId))
		{
			var permission = await _permissionRepository.GetByIdAsync(updateMenuDto.PermissionId, cancellationToken);
			if (permission == null)
			{
				throw new PermissionNotFoundException(updateMenuDto.PermissionId);
			}
		}

		menu.Update(
			updateMenuDto.MenuName,
			updateMenuDto.ParentMenuId,
			updateMenuDto.Icon,
			updateMenuDto.Url,
			updateMenuDto.PermissionId,
			updateMenuDto.SortOrder,
			updateMenuDto.IsVisible);

		_menuRepository.Update(menu);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return MapToMenuDto(menu);		
	}

	public async Task<bool> DeleteMenuAsync(string menuId, CancellationToken cancellationToken = default)
	{
		var menu = await _menuRepository.GetByIdAsync(menuId, cancellationToken)
			?? throw new DomainException($"{menuId}功能選單不存在！");

		var childMenus = await _menuRepository.GetChildMenusAsync(menuId, cancellationToken);
		if (childMenus.Any())
		{
			throw new DomainException($"無法刪除功能選單{menuId}，因為還有子功能選單！");
		}

		_menuRepository.Remove(menu);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> ActivateMenuAsync(string menuId, CancellationToken cancellationToken = default)
	{
		var menu = await _menuRepository.GetByIdAsync(menuId, cancellationToken)
			?? throw new DomainException($"{menuId}功能選單不存在！");

		menu.SetVisible(true);
		_menuRepository.Update(menu);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> DeactivateMenuAsync(string menuId, CancellationToken cancellationToken = default)
	{
		var menu = await _menuRepository.GetByIdAsync(menuId, cancellationToken)
			?? throw new DomainException($"{menuId}功能選單不存在！");

		menu.SetVisible(false);
		_menuRepository.Update(menu);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	private static MenuDto MapToMenuDto(Menu menu)
	{
		return new MenuDto(
			menu.MenuId,
			menu.ParentMenuId,
			menu.MenuName,
			menu.Icon,
			menu.Url,
			menu.PermissionId,
			menu.SortOrder,
			menu.IsVisible,
			menu.CreatedAt,
			menu.ModifiedAt,
			null);
	}
}
	
