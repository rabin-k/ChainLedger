using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Abstractions
{
    /// <summary>
    /// Defines the contract for a consensus mechanism.
    /// </summary>
    public interface IConsensusManager
    {
        /// <summary>
        /// Performs Proof-of-Work to find a valid nonce for the given block data.
        /// </summary>
        /// <param name="data">The data used for hashing.</param>
        /// <returns>A valid nonce that meets the difficulty requirement.</returns>
        int PerformProofOfWork(string data);

        /// <summary>
        /// Validates if the given block hash meets the consensus difficulty.
        /// </summary>
        /// <param name="hash">The hash to validate.</param>
        /// <returns>True if the hash meets the difficulty requirement; otherwise, false.</returns>
        bool ValidateProofOfWork(string hash);
    }
}
