using AQMTemplate.Application.Services.Interfaces.Auth;
using AQMTemplate.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AQMTemplate.Infrastructure.Services.Security.Auth
{
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtSettings _jwtSettings;

		public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
		{
			_jwtSettings = jwtSettings.Value;
		}

		public string GenerateToken(string userId, string account, IList<string> roles, IList<string> permissions)
		{
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha512);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userId),
				new Claim(ClaimTypes.Name, account)
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			foreach (var permission in permissions)
			{
				claims.Add(new Claim("permission", permission));
			}

			var securityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				signingCredentials: signingCredentials,
				expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes));

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}
	}
}
