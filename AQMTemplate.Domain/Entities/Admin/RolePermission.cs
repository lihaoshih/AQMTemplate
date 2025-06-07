using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Entities.Admin
{
	public class RolePermission
	{
		public string RoleId { get; set; } = string.Empty;
		public string PermissionId { get; set; } = string.Empty;

		public virtual Role Role { get; set; } = null!;
		public virtual Permission Permission { get; set; } = null!;

		private RolePermission() { }

		public RolePermission(string roleId, string permissionId) 
		{
			RoleId = roleId; 
			PermissionId = permissionId; 
		}
	}
}
