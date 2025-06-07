using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.DTOs.Auth;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Auth;
using AQMTemplate.Domain.Exceptions.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using AQMTemplate.Domain.Interfaces.Security.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AQMTemplate.Application.Services.Implementations.Auth
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IPermissionRepository _permissionRepository;
		private readonly IPasswordService _passwordService;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUnitOfWork _unitOfWork;

		public AuthService(
			IUserRepository userRepository, 
			IRoleRepository roleRepository, 
			IPermissionRepository permissionRepository, 
			IPasswordService passwordService, 
			IJwtTokenGenerator jwtTokenGenerator, 
			IHttpContextAccessor httpContextAccessor,
			IUnitOfWork unitOfWork)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_permissionRepository = permissionRepository;
			_passwordService = passwordService;
			_jwtTokenGenerator = jwtTokenGenerator;
			_httpContextAccessor = httpContextAccessor;
			_unitOfWork = unitOfWork;
		}

		public async Task<AuthResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.GetByAccountAsync(loginDto.Account, cancellationToken);
			if (user == null)
			{
				return null;
			}

			if (user.IsLocked || !user.IsActive)
			{
				return null;
			}

			if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordSalt, user.PasswordHash))
			{
				user.RecordLoginAttempt(false);
				_userRepository.Update(user);
				await _unitOfWork.SaveChangesAsync(cancellationToken);
				return null;
			}

			// login successfully, renew login time
			user.RecordLoginAttempt(true);
			_userRepository.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// get roles and permissions
			var roles = await _roleRepository.GetByUserIdAsync(user.UserId, cancellationToken);
			var permissions = await _permissionRepository.GetByUserIdAsync(user.UserId, cancellationToken);

			var roleNames = roles.Select(r => r.RoleName).ToList();
			var permissionNames = permissions.Select(p => p.PermissionName).ToList();

			var token = _jwtTokenGenerator.GenerateToken(
				user.UserId,
				user.Account,
				roleNames,
				permissionNames);

			return new AuthResultDto(
				user.UserId,
				user.Account,
				user.DisplayName,
				token,
				roleNames,
				permissionNames);
		}

		public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.GetByIdAsync(changePasswordDto.UserId, cancellationToken) ?? throw new UserNotFoundException(changePasswordDto.UserId);

			if (!_passwordService.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordSalt, user.PasswordHash))
			{
				return false;
			}

			var (passwordSalt, passwordHash) = _passwordService.HashPassword(changePasswordDto.NewPassword);
			user.UpdatePassword(passwordSalt, passwordHash);

			_userRepository.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}

		public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellatinToken = default)
		{
			var userId = GetCurrentUserId();
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("User is not authenticated!");
			}

			var user = await _userRepository.GetByIdAsync(userId, cancellatinToken)
				?? throw new UserNotFoundException(userId);

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

		public async Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default)
		{
			var userId = GetCurrentUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return false;
			}

			var permissions = await _permissionRepository.GetByUserIdAsync(userId, cancellationToken);
			return permissions.Any(p => p.PermissionName == permissionName);
		}

		private string? GetCurrentUserId()
		{
			return _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
