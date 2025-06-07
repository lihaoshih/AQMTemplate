using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AQMTemplate.Infrastructure.Persistence.Repositories.Admin
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _appDbContext;
		private readonly ILogger<UserRepository> _logger;

		public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger)
		{
			_appDbContext = appDbContext;
			_logger = logger;
		}

		public async Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
		}

		public async Task<User?> GetByAccountAsync(string account, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.Account == account, cancellationToken);
		}
		
		public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			_logger.LogInformation("UserRepository.GetAllAsync");
			var result = await _appDbContext.Users.ToListAsync();
//			var result = await _appDbContext.Users.ToListAsync(cancellationToken);
			_logger.LogInformation($"UserRepository.GetAllAsync: Fetched {result.Count()} users from DbContext");
			if (result.Any())
			{
				_logger.LogInformation("UserRepository.GetAllAsync: First user ID: {UserId}, Account: {UserAccount}", result.First().UserId, result.First().Account);
			}

			return result;
		}

		public async Task<IEnumerable<User>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return await _appDbContext.Users
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
				.ToListAsync(cancellationToken);
		}



		public async Task AddAsync(User user, CancellationToken cancellationToken = default)
		{
			await _appDbContext.Users.AddAsync(user, cancellationToken);
		}
		public void Update(User user)
		{
			_appDbContext.Users.Update(user);
		}

		public void Remove(User user)
		{
			_appDbContext.Users.Remove(user);
		}

	}
}
