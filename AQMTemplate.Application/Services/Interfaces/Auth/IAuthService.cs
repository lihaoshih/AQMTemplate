using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.DTOs.Auth;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Application.Services.Interfaces.Auth;

public interface IAuthService
{
	Task<AuthResultDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
	Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);
	Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);
	Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
}
