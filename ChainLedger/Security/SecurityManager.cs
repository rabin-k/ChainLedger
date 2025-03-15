using ChainLedger.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Security
{
    /// <summary>
    /// Manages security functions such as digital signatures and verification.
    /// Uses RSA for signing and verifying block authenticity.
    /// </summary>
    public class SecurityManager : ISecurityManager
    {
        private readonly RSA _rsa;

        /// <summary>
        /// Initializes a new instance of the SecurityManager class with a generated RSA key pair.
        /// </summary>
        public SecurityManager()
        {
            _rsa = RSA.Create(2048);
        }

        /// <summary>
        /// Signs a given message using the private key.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <returns>The digital signature as a base64 string.</returns>
        public string SignData(string message)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);
            byte[] signature = _rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signature);
        }

        /// <summary>
        /// Verifies a digital signature using the public key.
        /// </summary>
        /// <param name="message">The original message.</param>
        /// <param name="signature">The digital signature to verify.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        public bool VerifySignature(string message, string signature)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);
            byte[] signatureBytes = Convert.FromBase64String(signature);
            return _rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
