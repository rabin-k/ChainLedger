using ChainLedger.Abstractions;
using ChainLedger.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


//[assembly: InternalsVisibleTo("ChainLedger.Tests")]
namespace ChainLedger.Models
{
    /// <summary>
    /// Represents a single block in the blockchain.
    /// Each block contains task-related data and maintains integrity through cryptographic hashing.
    /// </summary>
    internal class Block<T> : IBlock<T>
    {
        /// <summary>
        /// The position of the block within the blockchain.
        /// </summary>
        public int Index { init; get; }

        /// <summary>
        /// The timestamp when the block was created.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Unique identifier for the data associated with this block.
        /// </summary>

        public string Id { get; }
        /// <summary>
        /// The hash of the previous block in the chain.
        /// Ensures continuity and integrity of the blockchain.
        /// </summary>
        public string PreviousHash { init; get; }

        /// <summary>
        /// The task data stored in this block.
        /// </summary>
        public BlockData<T> Data { init; get; }

        /// <summary>
        /// Nonce value for security
        /// </summary>
        public int Nonce { get; }

        /// <summary>
        /// Digital signature of the block
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// The computed SHA-256 hash of the block.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// The string value for the data
        /// </summary>
        private string StringData => Data.ToString() ?? "";

        /// <summary>
        /// Formatted data for computing the hash value
        /// </summary>
        public string HashInput => $"{Index}|{Timestamp}|{Id}|{PreviousHash}|{StringData}|{Nonce}";

        /// <summary>
        /// Initializes a new instance of the Block class.
        /// </summary>
        /// <param name="index">The block's index in the chain.</param>
        /// <param name="previousHash">Hash of the previous block.</param>
        /// <param name="data">Task-related data.</param>
        internal Block(int index, string previousHash, T data, int nonce)
        {
            Index = index;
            Timestamp = DataUtilities.GetUtcNow();
            Id = DataUtilities.GetGuid().ToString();
            PreviousHash = previousHash;
            Data = BlockData<T>.Factory.CreateData(data, index);
            Nonce = nonce;
            Hash = ComputeHash();
        }

        /// <summary>
        /// Computes the hash of the block using the HashingUtility.
        /// </summary>
        /// <returns>A hexadecimal string representing the hash.</returns>
        public string ComputeHash()
        {
            return HashingUtility.ComputeSha256Hash(HashInput);
        }
    }
}
