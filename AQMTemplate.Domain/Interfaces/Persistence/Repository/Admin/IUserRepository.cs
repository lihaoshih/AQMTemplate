using AQMTemplate.Domain.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IUserRepository
	{

		Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
		Task<User?> GetByAccountAsync(string account,  CancellationToken cancellationToken = default);
		Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<User>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);
		Task AddAsync(User user, CancellationToken cancellationToken = default);
		void Update(User user);
		void Remove(User user);
	}
}
