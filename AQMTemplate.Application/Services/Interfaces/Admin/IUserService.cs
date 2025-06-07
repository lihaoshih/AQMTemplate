using AQMTemplate.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Application.Services.Interfaces.Admin;

public interface IUserService
{
	Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
	Task<UserDto> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default);
	Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleId, CancellationToken cancellationToken = default);
	Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
	Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
	Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
	Task<bool> ActivateUserAsync(string userId, CancellationToken cancellationToken = default);
	Task<bool> DeactivateUserAsync(string userId, CancellationToken cancellationToken = default);
	Task<bool> UnlockUserAsync(string userId, CancellationToken cancellationToken = default);
	Task<bool> AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken = default);
	Task<bool> RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken = default);
	Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
}
