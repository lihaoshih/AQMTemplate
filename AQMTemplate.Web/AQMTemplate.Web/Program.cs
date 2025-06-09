global using FastEndpoints;
global using Microsoft.EntityFrameworkCore;
global using Syncfusion.Blazor;
using AQMTemplate.Application;
using AQMTemplate.Application.Services.Implementations.Admin;
using AQMTemplate.Application.Services.Implementations.Auth;
using AQMTemplate.Application.Services.Interfaces;
using AQMTemplate.Application.Services.Interfaces.Admin;
using AQMTemplate.Application.Services.Interfaces.Auth;
using AQMTemplate.Domain.Common.Security;
using AQMTemplate.Domain.Enums.Security;
using AQMTemplate.Domain.Interfaces.Persistence.Repository.Admin;
using AQMTemplate.Domain.Interfaces.Security.Auth;
using AQMTemplate.Infrastructure;
using AQMTemplate.Infrastructure.Persistence;
using AQMTemplate.Infrastructure.Persistence.Repositories;
using AQMTemplate.Infrastructure.Persistence.Repositories.Admin;
using AQMTemplate.Infrastructure.Services.Security.Auth;
using AQMTemplate.Web;
using AQMTemplate.Web.Components;
using AQMTemplate.Web.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Utilities.DTOs.Security;
using Utilities.Services.Security;


var builder = WebApplication.CreateBuilder(args);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg1Nzc5OUAzMjM5MmUzMDJlMzAzYjMyMzkzYlUyK3YzNmtvK3NzMmRDVzFDeVovQkJlU0tqZEpCSHcwMVdXYWt6ZnJONTQ9");

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerDocument();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("NpgConnection"),
    npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    ) // Recommended for PostgreSQL
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
			ValidAudience = builder.Configuration["JwtSettings:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ??
				throw new InvalidOperationException("JWT Secret is not configured.")))
		};
	});

//builder.Services.AddWebServices();


builder.Services.AddOpenApiDocument(settings =>
{
	settings.Title = "AQMTemplate API (NSwag)";
	settings.Version = "1.0.0";
});

if (builder.Environment.IsDevelopment())
{
	builder.Services.AddCors(options =>
	{
		options.AddDefaultPolicy(builder =>
		{
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
		});
	});
}

var app = builder.Build();

app.UseFastEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseOpenApi();
	app.UseSwaggerUi();

	app.UseWebAssemblyDebugging();
	app.UseCors();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(AQMTemplate.Web.Client._Imports).Assembly);

app.Run();
