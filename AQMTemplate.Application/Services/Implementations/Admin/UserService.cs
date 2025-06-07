using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Domain.Exceptions;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Admin;
using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Domain.Exceptions.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using AQMTemplate.Domain.Interfaces.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AQMTemplate.Application.Services.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace AQMTemplate.Application.Services.Implementations.Admin;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;
	private readonly IRoleRepository _roleRepository;
	private readonly IUserRoleRepository _userRoleRepository;
	private readonly IPasswordService _passwordService;
	private readonly ILogger<UserService> _logger;
	private readonly IUnitOfWork _unitOfWork;

	public UserService(IUserRepository userRepository, 
		IRoleRepository roleRepository, 
		IUserRoleRepository userRoleRepository,
		IPasswordService passwordService,
		IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_roleRepository = roleRepository;
		_userRoleRepository = userRoleRepository;
		_passwordService = passwordService;
		_unitOfWork = unitOfWork;
	}

	public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken) 
			?? throw new UserNotFoundException(userId);

		return MapToDto(user);
	}

	public async Task<UserDto> GetUserByAccountAsync(string account, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByAccountAsync(account, cancellationToken) 
			?? throw new UserNotFoundException(account);

		return MapToDto(user);
	}

	public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
	{
		var users = await _userRepository.GetAllAsync(cancellationToken);
		return users.Select(MapToDto);
	}

	public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleId, CancellationToken cancellationToken = default)
	{
		var users = await _userRepository.GetByRoleIdAsync(roleId, cancellationToken);
		return users.Select(MapToDto);
	}

	public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
	{
		var existingUser = await _userRepository.GetByAccountAsync(createUserDto.Account, cancellationToken);
		if (existingUser != null)
		{
			throw new DomainException($"使用者帳號{createUserDto}已存在");
		}

		var (passwordSalt, passwordHash) = _passwordService.HashPassword(createUserDto.Password);

		var user = new User(
			createUserDto.Account,
			passwordSalt,
			passwordHash,
			createUserDto.UserName,
			createUserDto.Gender);

		user.UpdateProfile(
			createUserDto.Department,
			createUserDto.JobTitle,
			createUserDto.DisplayName,
			null,
			createUserDto.Alias,
			createUserDto.IdNumber,
			createUserDto.Birthday,
			createUserDto.PostalCode,
			createUserDto.MailingAddress,
			createUserDto.Email,
			createUserDto.Phone);

		user.SetAnnotation(createUserDto.Annotation);

		await _userRepository.AddAsync(user, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return MapToDto(user);
	}

	public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		user.UpdateProfile(
			updateUserDto.Department,
			updateUserDto.JobTitle,
			updateUserDto.DisplayName,
			updateUserDto.AvatarUrl,
			updateUserDto.Alias,
			updateUserDto.IdNumber,
			updateUserDto.Birthday,
			updateUserDto.PostalCode,
			updateUserDto.MailingAddress,
			updateUserDto.Email,
			updateUserDto.Phone);

		user.SetAnnotation(updateUserDto.Annotation);

		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return MapToDto(user);
	}

	public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		_userRepository.Remove(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> ActivateUserAsync(string userId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		user.SetActive(true);
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> DeactivateUserAsync(string userId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		user.SetActive(false);
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> UnlockUserAsync(string userId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		user.Unlock();
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		var userRole = new UserRole(userId, roleId);
		await _userRoleRepository.AddAsync(userRole, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		await _userRoleRepository.RemoveAsync(userId, roleId, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
	{
		var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
			?? throw new UserNotFoundException(userId);

		var roles = await _roleRepository.GetByUserIdAsync(userId, cancellationToken);
		return roles.Select(role => new RoleDto(
			role.RoleId,
			role.RoleName,
			role.Description,
			role.CreatedAt,
			role.ModifiedAt));
	}

	private static UserDto MapToDto(User user)
	{
		return new UserDto(
			user.UserId,
			user.Account,
			user.Department,
			user.JobTitle,
			user.UserName,
			user.DisplayName,
			user.AvatarUrl,
			user.Alias,
			user.Gender,
			user.IdNumber,
			user.Birthday,
			user.PostalCode,
			user.MailingAddress,
			user.Email,
			user.Phone,
			user.IsActive,
			user.IsLocked,
			user.FailedLoginAttempt,
			user.LastLogin,
			user.Annotation,
			user.CreatedAt,
			user.ModifiedAt);
	}
}
