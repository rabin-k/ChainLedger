using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Tests.Helpers
{
    public static class SecurityHelper
    {
        private static readonly RSA _rsa;
        static SecurityHelper()
        {
            _rsa = RSA.Create(2048);
        }
        //public static string SignData(string message)
        //{
        //    byte[] dataBytes = Encoding.UTF8.GetBytes(message);
        //    byte[] signature = _rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        //    return Convert.ToBase64String(signature);
        //}

        public static bool VerifySignature(string message, string signature)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);
            byte[] signatureBytes = Convert.FromBase64String(signature);
            return _rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
