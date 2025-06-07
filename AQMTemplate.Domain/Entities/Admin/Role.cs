namespace AQMTemplate.Domain.Entities.Admin;

public class Role
{
	public int Id { get; set; }
	public string RoleId { get; set; } = null!;
	public string RoleName { get; set; } = null!;
	public string? Description { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }

	//public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
	//public virtual ICollection<User> Users { get; set; } = new List<User>();

	public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
	public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

	private Role() { }

	public Role(string roleName, string? description = null)
	{
		RoleName = roleName;
		Description = description;
		CreatedAt = DateTime.Now;
		ModifiedAt = DateTime.Now;
	}

	public void Update(string roleName, string? description = null)
	{
		RoleName = roleName;
		Description = description;
		ModifiedAt = DateTime.Now;
	}
}
