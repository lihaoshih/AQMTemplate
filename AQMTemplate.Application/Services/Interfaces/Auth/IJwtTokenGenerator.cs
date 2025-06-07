namespace AQMTemplate.Application.Services.Interfaces.Auth
{
	public interface IJwtTokenGenerator
	{
		public string GenerateToken(string userId, string account, IList<string> roles, IList<string> permissions);
	}
}
