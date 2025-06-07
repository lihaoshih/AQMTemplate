namespace AQMTemplate.Domain.Entities.Admin;

public class Permission
{
	public int Id { get; set; }
	public string PermissionId { get; set; } = null!;
	public string PermissionName { get; set; } = null!;
	public string? Description { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }

	public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
	//public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
	public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

	private Permission() { }

	public Permission(string permissionName, string? description = null)
	{
		PermissionName = permissionName;
		Description = description;
		CreatedAt = DateTime.Now;
		ModifiedAt = DateTime.Now;		
	}

	public void Update(string permissionName, string? description = null)
	{
		PermissionName = permissionName;
		Description = description;
		ModifiedAt = DateTime.Now;
	}

}
