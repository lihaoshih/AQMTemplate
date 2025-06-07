using AQMTemplate.Domain.Entities.Admin;

namespace AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin
{
	public interface IMenuRepository
	{
		Task<Menu?> GetByIdAsync(string menuId, CancellationToken cancellationToken =  default);
		Task<IEnumerable<Menu>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<Menu>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
		Task<IEnumerable<Menu>> GetRootMenusAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<Menu>> GetChildMenusAsync(string parentMenuId, CancellationToken cancellationToken = default);
		Task AddAsync(Menu menu, CancellationToken cancellationToken = default);
		void Update(Menu menu);
		void Remove(Menu menu);
	}
}
