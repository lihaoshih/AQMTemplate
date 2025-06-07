using Utilities.DTOs.Security;
using Utilities.Services.Security;

namespace AQMTemplate.Web.Endpoints.Utilities.Security
{
	public class CryptographyEndpoint : Endpoint<CryptographyRequest, CryptographyResponse>
	{
		public override void Configure()
		{
			Post("/api/cryptography");
			AllowAnonymous();
		}

		public override async Task HandleAsync(CryptographyRequest request, CancellationToken cancellationToken) =>
			await SendAsync(CryptographyService.Crypto(request), cancellation: cancellationToken);
	}
}
