using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Exceptions.Admin
{
	public class UserNotFoundException : DomainException
	{
		public UserNotFoundException(string account) : base($"找不到帳號為{account}的使用者") { }
	}

	public class RoleNotFoundException : DomainException
	{
		public RoleNotFoundException(string roleId) : base($"找不到角色ID：{roleId}") { }
	}

	public class PermissionNotFoundException : DomainException
	{
		public PermissionNotFoundException(string permissionId) : base($"找不到權限ID：{permissionId}")	{ }
	}
}
