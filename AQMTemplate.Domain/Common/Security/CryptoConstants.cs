using AQMTemplate.Domain.Enums.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Common.Security
{
	public static class CryptoConstants
	{
		public const string PassPhrase = "A2vgzN0iltxSBaRiAvjRSA==";
		public const CryptoAlgorithm Algorithm = CryptoAlgorithm.AES;
		public const CipherModeEnum CipherMode = CipherModeEnum.GCM;
		public const PaddingModeEnum PaddingMode = PaddingModeEnum.PKCS7;
		public const HashingAlgorithm Hash = HashingAlgorithm.SHA512; 
	}
}
