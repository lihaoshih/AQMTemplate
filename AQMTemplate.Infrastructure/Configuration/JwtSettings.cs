using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Infrastructure.Configuration
{
	public class JwtSettings
	{
		public const string SectionName = "JwtSettings";
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public int AccessTokenExpirationMinutes { get; set; } 
		public int RefreshTokenExpirationDays { get; set; } 
	}
}
