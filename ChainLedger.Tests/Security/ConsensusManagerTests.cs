using ChainLedger.Security;
using ChainLedger.Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Tests.Security
{
    [TestFixture]
    public class ConsensusManagerTests
    {
        private const int _difficulty = 3;
        private readonly string targetPrefix = new string('0', _difficulty);
        private ConsensusManager _consensusManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _consensusManager = new ConsensusManager(_difficulty);
        }

        [Test]
        public void Should_Generate_Correct_Nonce_And_Validate()
        {
            var testData = "Test Data";
            var nonce = _consensusManager.PerformProofOfWork(testData);
            Assert.That(nonce, Is.GreaterThan(-1), "Nonce value should be computed");

            var hash = HashingUtility.ComputeSha256Hash($"{testData}|{nonce}");
            Assert.That(hash, Does.StartWith(targetPrefix), "Hash value should match the complexity");

            var validationResult = _consensusManager.ValidateProofOfWork(hash);
            Assert.That(validationResult, Is.True);
        }

        [Test]
        [TestCase("012sadihaeifha", false)]
        [TestCase("000sadihaeifha", true)]
        public void Should_Validate_Hash(string hash, bool result)
        {
            var validationResult = _consensusManager.ValidateProofOfWork(hash);
            Assert.That(validationResult, Is.EqualTo(result), $"Validated correctly by returning {validationResult}.");
        }
    }
}
