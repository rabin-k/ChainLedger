using ChainLedger.Abstractions;
using ChainLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ChainLedger.Tests.Helpers;
using ChainLedger.Services;

namespace ChainLedger.Tests.Service
{
    [TestFixture]
    public class BlockChainTests
    {
        private IBlockChain<string> _blockchain;
        private Mock<IConsensusManager> _consensusManagerMock;
        private Mock<ISecurityManager> _securityManagerMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _consensusManagerMock = new Mock<IConsensusManager>();
            _securityManagerMock = new Mock<ISecurityManager>();
            _consensusManagerMock.Setup(m => m.ValidateProofOfWork(It.IsAny<string>())).Returns(true);
            _blockchain = new BlockChain<string>(_consensusManagerMock.Object, _securityManagerMock.Object);
        }

        [Test]
        [Order(1)] // Must execute first since we are validating the Genesis block and count
        public void Should_Start_With_Genesis_Block()
        {
            Assert.That(_blockchain.Chain.Count, Is.EqualTo(1), "Blockchain should start with exactly one block (the genesis block).");
            Assert.That(_blockchain.Chain[0].PreviousHash, Is.EqualTo(""), "Genesis block should have a empty previous hash of ''.");
        }

        [Test]
        [Order(2)]
        public void Should_Add_New_Blocks_Correctly()
        {
            _securityManagerMock.Setup(x => x.SignData(It.IsAny<string>())).Returns("Valid Signature");
            _blockchain.AddBlock("First data");
            _blockchain.AddBlock("Second data");

            Assert.That(_blockchain.Chain.Count, Is.EqualTo(3), "Blockchain should contain three blocks after adding two.");
            Assert.That(_blockchain.GetLatestBlock().Data.GetStringValue(), Is.EqualTo("Second data"), "Latest block should have the correct  ID.");
            Assert.That(_blockchain.Chain[0].Hash, Is.EqualTo(_blockchain.Chain[1].PreviousHash), "PreviousHash of block 1 should be equal to hash of block 0.");
            Assert.That(_blockchain.Chain[1].Signature, Is.EqualTo("Valid Signature"), "The signature should be generated and valid.");
        }

        [Test]
        [Order(3)]
        public void Should_Return_True_If_Valid()
        {
            _securityManagerMock.Setup(x => x.VerifySignature(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _blockchain.AddBlock("Third data");

            Assert.That(_blockchain.IsChainValid(), Is.True, "Blockchain should be valid ");
        }

        [Test]
        [Order(4)]
        public void Should_Detect_Tampering_Hash()
        {
            var tamperedBlock = _blockchain.Chain[1];
            var hashProp = typeof(Block<string>).GetField("<Hash>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            hashProp?.SetValue(tamperedBlock, "FakeHash");

            Assert.That(_blockchain.IsChainValid(), Is.False, "Blockchain should be invalid after tampering hash of a block.");
        }

        [Test]
        [Order(5)]
        public void Should_Detect_Tampering_Data()
        {
            var tamperedBlock = _blockchain.Chain[1];
            if (tamperedBlock != null)
            {
                tamperedBlock = new Block<string>(tamperedBlock.Index, tamperedBlock.PreviousHash, "Tampered data", 1);
                _blockchain.Chain[1] = tamperedBlock;
            }

            Assert.That(_blockchain.IsChainValid(), Is.False, "Blockchain should be invalid after tampering with a block.");
        }

        [Test]
        // This test has to execute at last since we ar modifying the ValidateProofOfWork to return false
        // Currently it is executing last but if the behavior changes in future, use [Order] to make it run last
        [Order(6)]
        public void Should_Throw_Error_When_Invalid_Proof_Of_Work()
        {
            _consensusManagerMock.Setup(m => m.ValidateProofOfWork(It.IsAny<string>())).Returns(false);

            var ex = Assert.Throws<InvalidOperationException>(() => _blockchain.AddBlock("First data"), "Blockchain should throw error when proof of work is not validated");
            Assert.That(ex.Message, Is.EqualTo("Block does not meet consensus requirements."), "Message in exception should be as expected");
        }
    }
}
