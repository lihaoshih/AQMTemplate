using AQMTemplate.Application.DTOs.Admin;

namespace AQMTemplate.Application.Services.Interfaces.Admin;

public interface IMenuService
{
	Task<MenuDto> GetMenuByIdAsync(string menuId, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> GetAllMenusAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> GetRootMenusAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> GetChildMenusAsync(string parentMenuId, CancellationToken cancellationToken = default);
	Task<IEnumerable<MenuDto>> GetUserMenusAsync(string userId, CancellationToken cancellationToken = default);
	Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default);
	Task<MenuDto> UpdateMenuAsync(string menuId, UpdateMenuDto updateMenuDto, CancellationToken cancellationToken = default);
	Task<bool> DeleteMenuAsync(string menuId, CancellationToken cancellationToken = default);
	Task<bool> ActivateMenuAsync(string menuId, CancellationToken cancellationToken = default);
	Task<bool> DeactivateMenuAsync(string menuId, CancellationToken cancellationToken = default);
}
