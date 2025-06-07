using AQMTemplate.Domain.Entities.Admin;

namespace AQMTemplate.Application.DTOs.Admin;

//public class MenuDto
//{
//	public string MenuId { get; set; } = string.Empty;
//	public string? ParentMenuId { get; set; }
//	public string MenuName { get; set; } = string.Empty;
//	public string? Icon { get; set; }
//	public string? Url { get; set; }
//	public int SortOrder { get; set; }
//	public List<MenuDto> Children { get; set; } = new List<MenuDto>();
//	public bool HasChildren => Children.Any();
//}

public record MenuDto(
	string MenuId,
	string? ParentMenuId,
	string MenuName,
	string? Icon,
	string? Url,
	string? PermissionId,
	int SortOrder,
	bool IsVisible,
	DateTime CreatedAt,
	DateTime ModifiedAt,
	
	List<MenuDto>? Children);

public record CreateMenuDto(
	string MenuName,
	string? ParentMenuId,
	string? Icon,
	string? Url,
	string? PermissionId,
	int SortOrder,
	bool IsVisible);

public record UpdateMenuDto(
	string MenuName,
	string? ParentMenuId,
	string? Icon,
	string? Url,
	string? PermissionId,
	int SortOrder,
	bool IsVisible);