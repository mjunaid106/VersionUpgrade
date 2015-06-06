using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor.Interfaces;
using Processor.Test.MockObjects;

namespace Processor.Test
{
    [TestClass]
    public class SourceTests
    {
        private ISource _source;

        [TestInitialize]
        public void Initialise()
        {
            _source = new MockSource();
        }

        [TestMethod]
        public void CheckInitialValues()
        {
            Assert.IsNull(_source.OldVersion);
            Assert.IsNull(_source.NewVersion);
            Assert.IsNull(_source.RecordName);
        }

        [TestMethod]
        public void ReadData()
        {
            Assert.AreEqual("REM v1.1.0", _source.ReadData());
        }

        [TestMethod]
        public void WriteData_Failure()
        {
            bool isSuccess = _source.WriteData("test");
            Assert.AreEqual(false, isSuccess);
        }

        [TestMethod]
        public void WriteData_Success()
        {
            bool isSuccess = _source.WriteData("REM v1.2.0");
            Assert.AreEqual(true, isSuccess);
        }

        [TestMethod]
        public void Versions_Success()
        {
            _source.Versions("1.2.0", "1.3.0");
            Assert.AreEqual("1.2.0", _source.OldVersion);
            Assert.AreEqual("1.3.0", _source.NewVersion);
        }
    }
}