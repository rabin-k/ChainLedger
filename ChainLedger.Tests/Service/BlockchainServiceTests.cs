using ChainLedger.Abstractions;
using ChainLedger.Models;
using ChainLedger.Services;
using Moq;

namespace ChainLedger.Tests.Service
{
    [TestFixture]
    public class BlockchainServiceTests
    {
        private BlockchainService<string> _service;
        private Mock<IBlockChain<string>> _mockBlockChain;

        [SetUp]
        public void Setup()
        {
            _mockBlockChain = new Mock<IBlockChain<string>>();
            _service = new BlockchainService<string>(_mockBlockChain.Object);
        }

        [Test]
        public void Should_Add_Update_To_Blockchain()
        {
            var testData = new List<string> { "Test Data" };
            _mockBlockChain.Setup(x => x.AddBlock(It.IsAny<string>())).Verifiable();
            _mockBlockChain.SetupGet(x => x.Chain).Returns(TestChain(testData));

            foreach (var d in testData)
                _service.AddUpdate(d);

            var history = _service.GetHistory(b => b.Data.Value == "Test Data");

            _mockBlockChain.Verify(x => x.AddBlock(It.IsAny<string>()), Times.Once);
            Assert.That(history.Count, Is.EqualTo(1), "Data add/update should be recorded in the blockchain.");
        }

        private static readonly SourceData[] _testSource =
        {
            new(["Test Data 1", "Test Data 2"], "data",2 ),
            new(["Test Data 3", "Test Data 4"], "new", 0 )
        };

        [Test, TestCaseSource(nameof(_testSource))]

        //Generic support after .NET 6
        //[TestCase<string[], string, int>(["1"], "data", 2)]
        //public void Should_Retrieve_Data_History_With_Filter(string[] data, string search, int count)

        public void Should_Retrieve_Data_History_With_Filter(SourceData source)
        {
            _mockBlockChain.Setup(x => x.AddBlock(It.IsAny<string>())).Verifiable();
            _mockBlockChain.SetupGet(x => x.Chain).Returns(TestChain(source.Data));

            foreach (var d in source.Data)
                _service.AddUpdate(d);

            var filteredHistory = _service.GetHistory(b => b.Data.Value.Contains(source.SearchString, StringComparison.OrdinalIgnoreCase));

            _mockBlockChain.Verify(x => x.AddBlock(It.IsAny<string>()), Times.Exactly(source.Data.Count));
            Assert.That(filteredHistory, Has.Count.EqualTo(source.FilteredCount), "Only matching data updates should be returned.");
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Should_Validate_Blockchain_Integrity(bool result, bool expected)
        {
            _mockBlockChain.Setup(x => x.IsChainValid()).Returns(result);

            var isValid = _service.ValidateBlockchain();
            Assert.That(isValid, Is.EqualTo(expected));
        }


        #region Helper

        private IList<IBlock<T>> TestChain<T>(IList<T> data)
        {
            var testData = new List<IBlock<T>>();
            int i = 1;
            foreach (var d in data)
                testData.Add(new Block<T>(i++, "", d, 100+i));

            return testData;
        }

        public class SourceData
        {
            public IList<string> Data { get; set; }
            public string SearchString { get; set; }
            public int FilteredCount { get; set; }

            public SourceData(IList<string> data, string searchString, int filteredCount)
            {
                Data = data;
                SearchString = searchString;
                FilteredCount = filteredCount;
            }
        }

        #endregion
    }
}
