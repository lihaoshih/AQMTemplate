using Utitlites.DTOs.Auth;
using Utitlites.Services.Auth;

namespace AQMTemplate.Web.Endpoints.Utilities.Auth;

public class PasswordVerifyEndpoint : Endpoint<VerifyPasswordRequest, VerifyPasswordResponse>
{
    public override void Configure()
    {
        Post("/api/auth/verifypassword");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Test Password Verify";
            s.Description = "Test Password Verify";
        });
    }

    public override async Task HandleAsync(VerifyPasswordRequest req, CancellationToken ct)
    {
        var testPassword = PasswordHashingService.HashPassword("LooDxLonq%021$");
        bool isCorrect = PasswordHashingService.VerifyPassword(req.Password, testPassword.SaltBase64, testPassword.HashBase64);

        var response = new VerifyPasswordResponse
        {
            IsSuccessful = isCorrect,
            Message = isCorrect ? "Password Verified" : "Password Not Verified",
            RetrievedSalt = testPassword.SaltBase64,
            RetrivedHash = testPassword.HashBase64
        };
        
        await SendAsync(response, cancellation:ct);
    }
}