using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.DTOs.Auth;
using Microsoft.JSInterop;

namespace AQMTemplate.Web.Services.Implementations;

public class WebAuthService
{
	private readonly HttpClient _httpClient;
	private readonly IJSRuntime _jSRuntime;

	public WebAuthService(HttpClient httpClient, IJSRuntime jSRuntime)
	{
		_httpClient = httpClient;
		_jSRuntime = jSRuntime;
	}

	public async Task<AuthResultDto?> LoginAsync(LoginDto loginDto)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

			if (response.IsSuccessStatusCode)
			{
				var authResult = await response.Content.ReadFromJsonAsync<AuthResultDto>();
				return authResult;
			}

			return null;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Login error: {ex.Message}");
			return null;
		}
	}

	public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
	{
		try
		{
			await SetAuthorizationHeader();
			var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", changePasswordDto);
			return response.IsSuccessStatusCode;
		}
		catch
		{
			return false;
		}
	}

	public async Task<UserDto?> GetCurrentUserAsync()
	{
		try
		{
			await SetAuthorizationHeader();
			var response = await _httpClient.GetAsync("api/auth/current-user");

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<UserDto>();
			}

			return null;
		}
		catch
		{
			return null;
		}
	}

	private async Task SetAuthorizationHeader()
	{
		var token = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

		if (!string.IsNullOrEmpty(token))
		{
			_httpClient.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
		}
	}
}
