using ChainLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Abstractions
{
    /// <summary>
    /// Defines the contract for a block in the blockchain.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the block.</typeparam>
    public interface IBlock<T>
    {
        /// <summary>
        /// Gets the position of the block within the blockchain.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the timestamp when the block was created.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the unique identifier for the data associated with this block.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the hash of the previous block in the chain.
        /// </summary>
        string PreviousHash { get; }

        /// <summary>
        /// Gets the data stored in this block.
        /// </summary>
        internal BlockData<T> Data { get; }

        /// <summary>
        /// Gets the computed SHA-256 hash of the block.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// Formatted data for computing the hash value
        /// </summary>
        string HashInput { get; }

        /// <summary>
        /// Nonce value for security
        /// </summary>
        int Nonce { get; }

        /// <summary>
        /// Digital signature of the block
        /// </summary>
        string Signature { get; set; }
    }
}
