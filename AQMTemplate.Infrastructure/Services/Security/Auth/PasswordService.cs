using AQMTemplate.Domain.Interfaces.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utitlites.Services.Auth;

namespace AQMTemplate.Infrastructure.Services.Security.Auth
{
	public class PasswordService : IPasswordService
	{
		public (string passwordSalt, string passwordHash) HashPassword(string plainPassword)
		{
			var hashResult = PasswordHashingService.HashPassword(plainPassword);

			return (hashResult.SaltBase64, hashResult.HashBase64);
		}

		public bool VerifyPassword(string plainPassword, string passwordSalt, string passwordHash)
		{
			return PasswordHashingService.VerifyPassword(plainPassword, passwordSalt, passwordHash);			
		}
	}
}
