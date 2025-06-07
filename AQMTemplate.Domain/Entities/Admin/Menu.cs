namespace AQMTemplate.Domain.Entities.Admin;

public partial class Menu
{
	public int Id { get; set; }
	public string MenuId { get; set; } = null!;
	public string? ParentMenuId { get; set; }
	public string MenuName { get; set; } = null!;
	public string? Icon { get; set; }
	public string? Url { get; set; }
	public string? PermissionId { get; set; }
	public int SortOrder { get; set; }
	public bool IsVisible { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }

	public virtual Menu? ParentMenu { get; set; }
	public virtual ICollection<Menu> ChildMenus { get; set; } = new List<Menu>();
	public virtual Permission? Permission { get; set; }

	private Menu() { }

	public Menu(string menuName, string? parentMenuId = null, string? icon = null,
		string? url = null, string? permissionId = null, int sortOrder = 0, bool isVisible = true)
	{
		MenuName = menuName;
		ParentMenuId = parentMenuId;
		Icon = icon;
		Url = url;
		PermissionId = permissionId;
		SortOrder = sortOrder;
		IsVisible = isVisible;
		CreatedAt = DateTime.Now;
		ModifiedAt = DateTime.Now;		
	}

	public void Update(string menuName, string? parentMenuId, string? icon,
		string? url, string? permissionId, int sortOrder, bool isVisible)
	{
		MenuName = menuName;
		ParentMenuId = parentMenuId;
		Icon = icon;
		Url = url;
		PermissionId = permissionId;
		SortOrder = sortOrder;
		IsVisible = isVisible;
		ModifiedAt = DateTime.Now;
	}

	public void SetVisible(bool isVisible)
	{
		IsVisible = isVisible;
		ModifiedAt = DateTime.Now;
	}

	public bool IsRootMenu()
	{
		return string.IsNullOrEmpty(ParentMenuId);
	}

	public bool HasChild()
	{
		return ChildMenus.Count > 0;
	}
}
