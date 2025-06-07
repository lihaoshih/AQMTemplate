using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Interfaces.Security.Auth
{
	public interface IPasswordService
	{
		(string passwordSalt, string passwordHash) HashPassword(string plainPassword);
		bool VerifyPassword(string plainPassword,  string passwordSalt, string passwordHash);
	}
}
