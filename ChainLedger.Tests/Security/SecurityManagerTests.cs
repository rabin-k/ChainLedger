using ChainLedger.Models;
using ChainLedger.Security;
using ChainLedger.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Tests.Security
{
    [TestFixture]
    public class SecurityManagerTests
    {
        private SecurityManager _securityManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _securityManager = new SecurityManager();
        }

        [Test]
        public void Should_Sign_Data_Correctly()
        {
            var previousHash = "0000";
            var data = "Sample Data";

            var block = new Block<string>(1, previousHash, data, 1);

            var signature = _securityManager.SignData(block.Hash);
            var verification = _securityManager.VerifySignature(block.Hash, signature);

            Assert.That(verification, Is.True, "The signature is verified successfully.");
        }

        [Test]
        public void Should_Return_False_If_Signature_Does_Not_Match()
        {
            var previousHash = "0000";
            var data = "Sample Data";

            var block1 = new Block<string>(1, previousHash, data, 1);
            var block2 = new Block<string>(2, block1.Hash, data, 1);

            var signature = _securityManager.SignData(block2.Hash);
            var verification = _securityManager.VerifySignature(block1.Hash, signature);

            Assert.That(verification, Is.False, "Signature is not verified when the conbination of Hash and Signature does not match.");
        }
    }
}
