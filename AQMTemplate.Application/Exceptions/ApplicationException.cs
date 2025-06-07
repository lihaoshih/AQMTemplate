using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Application.Exceptions
{
	public class ApplicationException : Exception
	{
		public ApplicationException(string message) : base(message)
		{ }

		public ApplicationException(string message, Exception innerException) : base(message, innerException) 
		{ }

		public class AuthenticationException : ApplicationException
		{
			public AuthenticationException(string message) : base(message)
			{
			}
		}

		public class AuthorizationException : ApplicationException
		{
			public AuthorizationException(string message) : base(message)
			{
			}
		}
	}
}
