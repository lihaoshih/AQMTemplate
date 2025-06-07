using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Auth;
using AQMTemplate.Application.Services.Interfaces.Persistence;
using AQMTemplate.Domain.Common.Security;
using AQMTemplate.Domain.Enums.Security;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using AQMTemplate.Domain.Interfaces.Security.Auth;
using AQMTemplate.Infrastructure.Configuration;
using AQMTemplate.Infrastructure.Persistence;
using AQMTemplate.Infrastructure.Persistence.Repositories;
using AQMTemplate.Infrastructure.Persistence.Repositories.Admin;
using AQMTemplate.Infrastructure.Services.Security.Auth;
using AQMTemplate.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities.DTOs.Security;
using Utilities.Services.Security;


namespace AQMTemplate.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		//var encryptedSQLPassword = configuration.GetConnectionString("NpgConnection")
		//	.Split("Password=")[1];

		//CryptographyRequest request = new CryptographyRequest
		//{
		//	Operation = CryptoOperation.Decrypt,
		//	InputText = encryptedSQLPassword,
		//	PassPhrase = CryptoConstants.PassPhrase,
		//	Algorithm = CryptoConstants.Algorithm,
		//	CipherMode = CryptoConstants.CipherMode,
		//	PaddingMode = CryptoConstants.PaddingMode
		//};

		//var decryptedSQLPassword = CryptographyService.Crypto(request).Result;
		//var connectionString = configuration.GetConnectionString("NpgConnection")
		//	.Split("Password=")[0] + $"Password='{decryptedSQLPassword}'";

		//services.AddDbContextPool<AppDbContext>(options =>
		//{
		//	options.UseNpgsql(configuration.GetConnectionString("NpgConnection"),
		//		b => b
		//		.SetPostgresVersion(17, 0)
		//		)
		//	.LogTo(Console.WriteLine, LogLevel.Information);

		//	//if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
		//	//{
		//	//	options.EnableSensitiveDataLogging();
		//	//}
		//});

		services.AddDbContext<AQMTemplateContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("MSSqlConnection")));

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();
		services.AddScoped<IPermissionRepository, PermissionRepository>();
		services.AddScoped<IMenuRepository, MenuRepository>();
		services.AddScoped<IUserRoleRepository, UserRoleRepository>();
		services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IPasswordService, PasswordService>();

		//services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
		services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

		services.AddHttpContextAccessor();
		return services;
	}
}
