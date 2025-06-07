global using FastEndpoints;
global using Microsoft.EntityFrameworkCore;
global using Syncfusion.Blazor;
using AQMTemplate.Application;
using AQMTemplate.Domain.Common.Security;
using AQMTemplate.Domain.Enums.Security;
using AQMTemplate.Infrastructure.Extensions;
using AQMTemplate.Infrastructure.Persistence;
using AQMTemplate.Web;
using AQMTemplate.Web.Components;
using AQMTemplate.Web.MiddleWare;
using FastEndpoints.Swagger;
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
builder.Services.AddSwaggerDocument();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "AQMTemplate FastEndpoints API",
		Version = "v1",
		Description = "AQMTemplate"
	});
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
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "FastEndpoint API V1");
		options.RoutePrefix = string.Empty;
	});

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
