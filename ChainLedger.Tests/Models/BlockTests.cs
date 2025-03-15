using ChainLedger.Models;
using ChainLedger.Tests.Helpers;
using ChainLedger.Utilities;

namespace ChainLedger.Tests.Models
{
    [TestFixture]
    public class BlockTests
    {
        [Test]
        public void Should_Initialize_Correctly()
        {
            var previousHash = "0000";
            var data = "Sample Data";
            var start = DateTime.UtcNow;

            var block = new Block<string>(1, previousHash, data, 0);
            var blockData = BlockData<string>.Factory.CreateData(data, 1);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(block.Index, Is.EqualTo(1));
                Assert.That(block.Data, Is.EqualTo(blockData).Using<BlockData<string>>((d1,d2) =>
                {
                    return d1.GetStringValue() == d2.GetStringValue();
                }));
                Assert.DoesNotThrow(() => Guid.TryParse(block.Id, out Guid guidResult));
                Assert.That(block.PreviousHash, Is.SameAs(previousHash));
                Assert.That(block.Timestamp, Is.InRange(start, DateTime.UtcNow));
                Assert.That(block.Hash, Is.Not.Null, "Block should initialize correctly");
            }
        }

        [Test]
        public void Should_Generate_Correct_Hash()
        {
            var previousHash = "0000";
            var data = "Sample Data";

            var block = new Block<string>(1, previousHash, data, 1);

            var expectedHash = HashHelper.ComputeHash(block.HashInput);
            Console.Write(block.Hash);
            Assert.That(block.Hash, Is.EqualTo(expectedHash), "Hash should be computed correctly.");
        }

        [Test]
        public void Hash_Should_Be_Unique_For_Different_Data()
        {
            var block1 = new Block<string>(1, "0", "First task data", 1);
            var block2 = new Block<string>(2, block1.Hash, "Second task data", 2);

            Assert.That(block2.Hash, Is.Not.EqualTo(block1.Hash), "Hashes should be unique for different blocks.");
        }

        [Test]
        public void Hash_Should_Be_Consistent_For_Same_Data()
        {
            //setup
            Func<DateTime> _originalGetUtcNow = DataUtilities.GetUtcNow;
            Func<Guid> _originalGetGuid = DataUtilities.GetGuid;
            Guid g = Guid.NewGuid();
            DateTime d = DateTime.UtcNow;
            DataUtilities.GetUtcNow = () => d;
            DataUtilities.GetGuid = () => g;

            var block1 = new Block<string>(1, "0", "Same task data", 1);
            var block2 = new Block<string>(1, "0", "Same task data", 1);

            Assert.That(block2.Hash, Is.EqualTo(block1.Hash), "Hashes should be the same for identical data.");

            //teardown
            DataUtilities.GetUtcNow = _originalGetUtcNow;
            DataUtilities.GetGuid = _originalGetGuid;
        }

        [Test]
        public void Hash_Should_Change_If_Data_Changes()
        {
            var block1 = new Block<string>(1, "0", "Original task data", 1);
            var block2 = new Block<string>(1, "0", "Modified task data", 2);

            Assert.That(block2.Hash, Is.Not.EqualTo(block1.Hash), "Hash should change when block data is modified.");
        }

        [Test]
        public void Should_Not_Throw_Error_When_ToString_Implemented()
        {
            var data = new SuccessClass { A = 1, B = 2 };

            Assert.DoesNotThrow(() => new Block<SuccessClass>(1, "0", data, 1));
        }

        [Test]
        public void Should_Throw_Error_When_ToString_Not_Implemented()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var data = new ErrorClass { A = 1, B = 2 };
                new Block<ErrorClass>(1, "0", data, 1);
            });
            Assert.That(ex.Message, Is.EqualTo("The type must override the ToString() method."));
        }
    }


    #region Helpers
    class ErrorClass
    {
        public int A { get; set; }
        public int B { get; set; }
    }

    class SuccessClass
    {
        public int A { get; set; }
        public int B { get; set; }

        public override string ToString()
        {
            return $"{A}{B}";
        }
    }

    #endregion
}
