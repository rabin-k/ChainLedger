using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Abstractions
{
    public interface ISecurityManager
    {
        /// <summary>
        /// Signs a given message using the private key.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <returns>The digital signature as a base64 string.</returns>
        string SignData(string message);

        /// <summary>
        /// Verifies a digital signature using the public key.
        /// </summary>
        /// <param name="message">The original message.</param>
        /// <param name="signature">The digital signature to verify.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        bool VerifySignature(string message, string signature);

    }
}
