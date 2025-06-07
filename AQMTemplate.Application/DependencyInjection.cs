using AQMTemplate.Application.Services.Interfaces.Admin;
using AQMTemplate.Application.Services.Implementations.Admin;
using Microsoft.Extensions.DependencyInjection;
using AQMTemplate.Application.Services.Interfaces.Auth;
using AQMTemplate.Application.Services.Implementations.Auth;

namespace AQMTemplate.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IRoleService, RoleService>();
		services.AddScoped<IPermissionService, PermissionService>();
		services.AddScoped<IMenuService, MenuService>();
		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}
