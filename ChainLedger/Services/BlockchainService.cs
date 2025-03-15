using ChainLedger.Abstractions;
using ChainLedger.Models;
using ChainLedger.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Services
{
    /// <summary>
    /// Provides a high-level API for interacting with the blockchain.
    /// Encapsulates blockchain operations and abstracts implementation details.
    /// </summary>
    internal class BlockchainService<T> : IBlockChainService<T>
    {
        private readonly IBlockChain<T> _blockchain;

        /// <summary>
        /// Initializes a new instance of the BlockchainService class.
        /// </summary>
        /// <param name="blockchain">The blockchain.</param>
        public BlockchainService(IBlockChain<T> blockchain)
        {
            _blockchain = blockchain;
        }

        /// <summary>
        /// Adds a new task update to the blockchain.
        /// </summary>
        /// <param name="taskId">The unique task identifier.</param>
        /// <param name="data">The task-related data.</param>
        public void AddUpdate(T data)
        {
            _blockchain.AddBlock(data);
        }

        /// <summary>
        /// Retrieves the full history of updates for a specific task.
        /// </summary>
        /// <param name="taskId">The unique task identifier.</param>
        /// <returns>A list of blocks associated with the task.</returns>
        public IList<IBlock<T>> GetHistory(Func<IBlock<T>, bool> filter)
        {
            return _blockchain.Chain.Where(filter).ToList();
        }

        /// <summary>
        /// Validates the blockchain's integrity and authenticity.
        /// </summary>
        /// <returns>True if the blockchain is valid; otherwise, false.</returns>
        public bool ValidateBlockchain()
        {
            return _blockchain.IsChainValid();
        }
    }
}
