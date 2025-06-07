using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AQMTemplate.Domain.Exceptions;
using AQMTemplate.Domain.Exceptions.Admin;

namespace AQMTemplate.Web.MiddleWare;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

	public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "處理請求時發生未處理的例外！");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var response = context.Response;
		response.ContentType = "application/json";

		var (statusCode, message) = exception switch
		{
			UserNotFoundException => (HttpStatusCode.NotFound, exception.Message),
			RoleNotFoundException => (HttpStatusCode.NotFound, exception.Message),
			PermissionNotFoundException => (HttpStatusCode.NotFound, exception.Message),
			DomainException => (HttpStatusCode.BadRequest, exception.Message),
			UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "未經授權的存取！"),
			_ => (HttpStatusCode.InternalServerError, "內部伺服器發生錯誤！")
		};

		response.StatusCode = (int)statusCode;

		var result = JsonSerializer.Serialize(new
		{
			success = false,
			message = message,
			statusCode = (int)statusCode
		});

		await response.WriteAsync(result);
	}
}
