using AQMTemplate.Domain.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IUserRoleRepository
	{
		Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
		Task RemoveAsync(string userId, string roleId, CancellationToken cancellationToken = default);
	}
}
