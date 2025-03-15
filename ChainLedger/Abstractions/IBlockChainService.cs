using ChainLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Abstractions
{
    public interface IBlockChainService<T>
    {
        /// <summary>
        /// Adds a new task update to the blockchain.
        /// </summary>
        /// <param name="data">The task-related data.</param>
        void AddUpdate(T data);

        /// <summary>
        /// Retrieves data history based on a user-defined filter function.
        /// </summary>
        /// <param name="filter">A lambda function to filter blocks.</param>
        /// <returns>A list of blocks matching the filter criteria.</returns>
        IList<IBlock<T>> GetHistory(Func<IBlock<T>, bool> filter);

        /// <summary>
        /// Validates the blockchain's integrity and authenticity.
        /// </summary>
        /// <returns>True if the blockchain is valid; otherwise, false.</returns>
        bool ValidateBlockchain();
    }
}
