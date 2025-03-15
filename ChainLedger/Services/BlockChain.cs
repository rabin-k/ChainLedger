using ChainLedger.Abstractions;
using ChainLedger.Models;
using ChainLedger.Utilities;
using System.Threading.Tasks;

namespace ChainLedger.Services
{
    /// <summary>
    /// Represents a generic blockchain that maintains a list of blocks.
    /// Ensures immutability, integrity, and validation of task updates.
    /// </summary>
    internal class BlockChain<T> : IBlockChain<T>
    {
        private readonly IConsensusManager _consensusManager;
        private readonly ISecurityManager _securityManager;

        /// <summary>
        /// The list of blocks in the blockchain.
        /// </summary>
        public IList<IBlock<T>> Chain { get; }

        /// <summary>
        /// Initializes a new blockchain with a genesis block.
        /// </summary>
        public BlockChain(IConsensusManager consensusManager, ISecurityManager securityManager)
        {
            // Initialize the blockchain with a genesis block
            Chain = new List<IBlock<T>> { CreateGenesisBlock() };
            _consensusManager = consensusManager;
            _securityManager = securityManager;
        }

        /// <summary>
        /// Creates the first block (genesis block) in the blockchain.
        /// </summary>
        /// <returns>The genesis block.</returns>
        private IBlock<T> CreateGenesisBlock()
        {
            return new Block<T>(0, "", default, 0);
        }

        /// <summary>
        /// Adds a new block to the blockchain.
        /// </summary>
        /// <param name="data">The input data.</param>
        public void AddBlock(T data)
        {
            var previousBlock = GetLatestBlock();

            int nonce = _consensusManager.PerformProofOfWork(previousBlock.HashInput);
            IBlock<T> newBlock = new Block<T>(previousBlock.Index + 1, previousBlock.Hash, data, nonce);

            if (!_consensusManager.ValidateProofOfWork(newBlock.Hash))
            {
                throw new InvalidOperationException("Block does not meet consensus requirements.");
            }
            string signature = _securityManager.SignData(newBlock.Hash);
            newBlock.Signature = signature;

            Chain.Add(newBlock);
        }

        /// <summary>
        /// Retrieves the latest block in the blockchain.
        /// </summary>
        /// <returns>The most recently added block.</returns>
        public IBlock<T> GetLatestBlock()
        {
            return Chain.Last();
        }

        /// <summary>
        /// Validates the integrity of the blockchain.
        /// Ensures that all blocks are properly linked and untampered.
        /// </summary>
        /// <returns>True if the blockchain is valid; otherwise, false.</returns>
        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                // Check if the current block's hash is valid
                if (currentBlock.Hash != HashingUtility.ComputeSha256Hash(currentBlock.HashInput))
                {
                    return false;
                }

                // Check if the previous hash is valid
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

                // Verify block signature
                if (!_securityManager.VerifySignature(currentBlock.Hash, currentBlock.Signature))
                {
                    return false;
                }
            }
            return true;
        }
    }
}