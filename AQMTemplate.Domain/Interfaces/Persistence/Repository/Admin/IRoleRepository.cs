using AQMTemplate.Domain.Entities.Admin;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IRoleRepository
	{
		Task<Role?> GetByIdAsync(string roleId, CancellationToken cancellationToken = default);
		Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<Role>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
		Task AddAsync(Role role, CancellationToken cancellationToken = default);
		void Update(Role role);
		void Remove(Role role);
	}
}
