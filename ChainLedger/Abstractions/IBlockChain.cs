using ChainLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Abstractions
{
    /// <summary>
    /// Defines the contract for a blockchain implementation.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the blockchain.</typeparam>
    public interface IBlockChain<T>
    {
        /// <summary>
        /// Gets the list of blocks in the blockchain.
        /// </summary>
        IList<IBlock<T>> Chain { get; }

        /// <summary>
        /// Retrieves the latest block in the blockchain.
        /// </summary>
        /// <returns>The most recently added block.</returns>
        IBlock<T> GetLatestBlock();

        /// <summary>
        /// Adds a new block to the blockchain.
        /// </summary>
        /// <param name="taskId">The ID of the task being added.</param>
        /// <param name="data">The task-related data.</param>
        void AddBlock(T data);

        /// <summary>
        /// Validates the integrity of the blockchain.
        /// </summary>
        /// <returns>True if the blockchain is valid; otherwise, false.</returns>
        bool IsChainValid();
    }
}
