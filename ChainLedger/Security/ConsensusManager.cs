using ChainLedger.Abstractions;
using ChainLedger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Security
{
    /// <summary>
    /// Manages the consensus mechanism for validating new blocks.
    /// Implements a basic Proof-of-Work (PoW) system.
    /// </summary>
    internal class ConsensusManager: IConsensusManager
    {
        private readonly int _difficulty;

        /// <summary>
        /// Initializes a new instance of the ConsensusManager class.
        /// </summary>
        /// <param name="difficulty">The number of leading zeros required in the hash for valid blocks.</param>
        public ConsensusManager(int difficulty = 4)
        {
            _difficulty = difficulty;
        }

        /// <summary>
        /// Performs Proof-of-Work to find a valid nonce for the given block data.
        /// </summary>
        /// <param name="data">The data used for hashing.</param>
        /// <returns>A valid nonce that meets the difficulty requirement.</returns>
        public int PerformProofOfWork(string data)
        {
            int nonce = 0;
            string hash;
            string targetPrefix = new string('0', _difficulty);

            do
            {
                nonce++;
                string input = $"{data}|{nonce}";
                hash = HashingUtility.ComputeSha256Hash(input);
            } while (!hash.StartsWith(targetPrefix));

            return nonce;
        }

        /// <summary>
        /// Validates if the given block hash meets the consensus difficulty.
        /// </summary>
        /// <param name="hash">The hash to validate.</param>
        /// <returns>True if the hash meets the difficulty requirement; otherwise, false.</returns>
        public bool ValidateProofOfWork(string hash)
        {
            string targetPrefix = new string('0', _difficulty);
            return hash.StartsWith(targetPrefix);
        }
    }
}

