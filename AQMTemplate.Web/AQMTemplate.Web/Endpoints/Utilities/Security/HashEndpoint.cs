using Utilities.DTOs.Security;
using Utilities.Services.Security;

namespace AQMTemplate.Web.Endpoints.Utilities.Security
{
	public class HashEndpoint : Endpoint<HashingRequest, HashingResponse>
	{
		public override void Configure()
		{
			Post("/api/hash");
			AllowAnonymous();
		}

		public override async Task HandleAsync(HashingRequest request, CancellationToken cancellationToken) => await SendAsync(CryptographyService.Hash(request), cancellation: cancellationToken);
	}
}
