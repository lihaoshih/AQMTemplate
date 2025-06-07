namespace AQMTemplate.Domain.Entities.Admin
{
	public class UserRole
	{
		public string UserId { get; set; } = string.Empty;
		public string RoleId { get; set; } = string.Empty;

		public virtual User User { get; set; } = null!;
		public virtual Role Role { get; set; } = null!;

		private UserRole() { }

		public UserRole(string userId, string roleId)
		{
			UserId = userId;
			RoleId = roleId;
		}
	}
}
