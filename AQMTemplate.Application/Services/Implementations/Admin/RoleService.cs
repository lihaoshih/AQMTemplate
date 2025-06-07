using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Domain.Exceptions.Admin;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Application.Services.Interfaces.Admin;

namespace AQMTemplate.Application.Services.Implementations.Admin;

public class RoleService : IRoleService
{
	private readonly IRoleRepository _roleRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IRolePermissionRepository _rolePermissionRepository;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RoleService(
		IRoleRepository roleRepository,
		IPermissionRepository permissionRepository,
		IRolePermissionRepository rolePermissionRepository,
		IUserRepository userRepository,
		IUnitOfWork unitOfWork)
	{
		_roleRepository = roleRepository;
		_permissionRepository = permissionRepository;
		_rolePermissionRepository = rolePermissionRepository;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<RoleDto> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		return MapToDto(role);			
	}

	public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
	{
		var roles = await _roleRepository.GetAllAsync(cancellationToken);
		return roles.Select(MapToDto);
	}

	public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto, CancellationToken cancellationToken = default)
	{
		var role = new Role(createRoleDto.RoleName, createRoleDto.Description);
		await _roleRepository.AddAsync(role, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return MapToDto(role);
	}

	public async Task<RoleDto> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		role.Update(updateRoleDto.RoleName, updateRoleDto.Description);
		_roleRepository.Update(role);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return MapToDto(role);
	}

	public async Task<bool> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken) 
			?? throw new RoleNotFoundException(roleId);

		_roleRepository.Remove(role);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> AddPermissionToRoleAsync(string roleId, string permissionId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken) 
			?? throw new RoleNotFoundException(roleId);

		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken)
			?? throw new PermissionNotFoundException(permissionId);

		var rolePermission = new RolePermission(roleId, permissionId);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken)
			?? throw new PermissionNotFoundException(permissionId);

		await _rolePermissionRepository.RemoveAsync(roleId, permissionId, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		var permissions = await _permissionRepository.GetByRoleIdAsync(roleId, cancellationToken);
		return permissions.Select(p => new PermissionDto(
			p.PermissionId,
			p.PermissionName,
			p.Description,
			p.CreatedAt,
			p.ModifiedAt));
	}

	public async Task<IEnumerable<UserDto>> GetRoleUsersAsync(string roleId, CancellationToken cancellationToken = default)
	{
		var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken)
			?? throw new RoleNotFoundException(roleId);

		var users = await _userRepository.GetByRoleIdAsync(roleId, cancellationToken);
		return users.Select(u => new UserDto(
			u.UserId,
			u.Account,
			u.Department,
			u.JobTitle,
			u.UserName,
			u.DisplayName,
			u.AvatarUrl,
			u.Alias,
			u.Gender,
			u.IdNumber,
			u.Birthday,
			u.PostalCode,
			u.MailingAddress,
			u.Email,
			u.Phone,
			u.IsActive,
			u.IsLocked,
			u.FailedLoginAttempt,
			u.LastLogin,
			u.Annotation,
			u.CreatedAt,
			u.ModifiedAt));
	}

	private static RoleDto MapToDto(Role role)
	{
		return new RoleDto(
			role.RoleId,
			role.RoleName,
			role.Description,
			role.CreatedAt,
			role.ModifiedAt);
	}
}
