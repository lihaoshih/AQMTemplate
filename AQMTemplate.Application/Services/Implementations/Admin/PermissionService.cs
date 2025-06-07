using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Admin;
using AQMTemplate.Domain.Entities.Admin;
using AQMTemplate.Domain.Exceptions.Admin;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;

namespace AQMTemplate.Application.Services.Implementations.Admin;

public class PermissionService : IPermissionService
{
	private readonly IPermissionRepository _permissionRepository;
	private readonly IRoleRepository _roleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public PermissionService(
		IPermissionRepository permissionRepository,
		IRoleRepository roleRepository,
		IUnitOfWork unitOfWork)
	{
		_permissionRepository = permissionRepository;
		_roleRepository = roleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<PermissionDto> GetPermissionByIdAsync(string permissionId, CancellationToken cancellationToken = default)
	{
		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken)
			?? throw new PermissionNotFoundException(permissionId);

		return MapToDto(permission);
	}

	public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
	{
		var permissions = await _permissionRepository.GetAllAsync(cancellationToken);
		return permissions.Select(MapToDto);
	}

	public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto, CancellationToken cancellationToken = default)
	{
		var permission = new Permission(createPermissionDto.PermissionName, createPermissionDto.Description);
		await _permissionRepository.AddAsync(permission, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return MapToDto(permission);
	}

	public async Task<PermissionDto> UpdatePermissionAsync(string permissionId, UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken = default)
	{
		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken)
			?? throw new PermissionNotFoundException($"Permission: {permissionId}");

		permission.Update(updatePermissionDto.PermissionName, updatePermissionDto.Description);
		_permissionRepository.Update(permission);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return MapToDto(permission);
	}

	public async Task<bool> DeletePermissionAsync(string permissionId, CancellationToken cancellationToken = default)
	{
		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken) 
			?? throw new PermissionNotFoundException($"Permission: {permissionId}");

		_permissionRepository.Remove(permission);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<IEnumerable<RoleDto>> GetPermissionRolesAsync(string permissionId, CancellationToken cancellationToken = default)
	{
		var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken) 
			?? throw new PermissionNotFoundException($"{permissionId} is not allowed");

		var roles = await _roleRepository.GetAllAsync(cancellationToken);
		return roles
			.Where(r => r.RolePermissions.Any(rp => rp.PermissionId == permissionId))
			.Select(r => new RoleDto(
				r.RoleId,
				r.RoleName,
				r.Description,
				r.CreatedAt,
				r.ModifiedAt));
	}

	private static PermissionDto MapToDto(Permission permission)
	{
		return new PermissionDto(
			permission.PermissionId,
			permission.PermissionName,
			permission.Description,
			permission.CreatedAt,
			permission.ModifiedAt);
	}
}
