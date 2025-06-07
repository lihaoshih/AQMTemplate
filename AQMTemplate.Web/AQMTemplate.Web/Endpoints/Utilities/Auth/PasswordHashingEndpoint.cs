using Utitlites.DTOs.Auth;
using Utitlites.Services.Auth;

namespace AQMTemplate.Web.Endpoints.Utilities.Auth
{
	public class PasswordHashingEndpoint : Endpoint<string, HashedPasswordResult>
	{
		public override void Configure()
		{
			Post("/api/passwordhash");
			AllowAnonymous();
		}

		public override async Task HandleAsync(string password, CancellationToken cancellationToken) => await SendAsync(PasswordHashingService.HashPassword(password), cancellation: cancellationToken);
	}
}
