using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace AQMTemplate.Web.Services.Implementations;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly IJSRuntime _jsRuntime;
	private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

	public CustomAuthenticationStateProvider(IJSRuntime jSRuntime)
	{
		_jsRuntime = jSRuntime;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

			if (string.IsNullOrEmpty(token))
			{
				return new AuthenticationState(_anonymous);
			}

			var claims = ParseClaimsFromJwt(token);
			var identity = new ClaimsIdentity(claims, "jwt");
			var user = new ClaimsPrincipal(identity);

			return new AuthenticationState(user);
		} catch
		{
			return new AuthenticationState(_anonymous);
		}
	}

	public void NotifyAuthenticationChanged()
	{
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public void NotifyLogout()
	{
		var authState = Task.FromResult(new AuthenticationState(_anonymous));
		NotifyAuthenticationStateChanged(authState);
	}

	private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
	{
		var claims = new List<Claim>();
		var payload = jwt.Split('.')[1];
		var jsonBytes = ParseBase64WithoutPadding(payload);
		var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

		if (keyValuePairs != null) {
			foreach (var kvp in keyValuePairs) {
				if (kvp.Key == ClaimTypes.Role)
				{
					if (kvp.Value is JsonElement element)
					{
						if (element.ValueKind == JsonValueKind.Array)
						{
							foreach (var role in element.EnumerateArray())
							{
								claims.Add(new Claim(ClaimTypes.Role, role.GetString() ?? string.Empty));
							}
						}
						else
						{
							claims.Add(new Claim(ClaimTypes.Role, element.GetString() ?? string.Empty));
						}
					}
				} else if (kvp.Key == "permission")
				{
					if (kvp.Value is JsonElement element)
					{
						if (element.ValueKind == JsonValueKind.Array)
						{
							foreach (var permission in element.EnumerateArray())
							{
								claims.Add(new Claim("permission", permission.GetString() ?? string.Empty));
							}
						}
						else
						{
							claims.Add(new Claim("permission", element.GetString() ?? string.Empty));
						}
					}
				}
				else
				{
					claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
				}
			}
		}

		return claims;
	}

	private byte[] ParseBase64WithoutPadding(string base64)
	{
		switch (base64.Length % 4)
		{
			case 2:
				base64 += "==";
				break;
			case 3:
				base64 += "=";
				break;
		}
		return Convert.FromBase64String(base64);
	}
}
