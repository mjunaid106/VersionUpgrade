using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor.Interfaces;
using Processor.Test.MockObjects;

namespace Processor.Test
{
    [TestClass]
    public class ManagerTests
    {
        private IIndex _index;
        private IManager _manager;
        private ISource _source;

        [TestInitialize]
        public void Initialise()
        {
            _source = new MockSource {OldVersion = "1.1.0", NewVersion = "1.2.0", RecordName = "InputFile.bat"};
            _index = new MockIndex("MockIndex.csv");
            _manager = new MockManager(_index);
            _manager.Source = _source;
        }

        [TestMethod]
        public void Update_WithShortVersion_FullFormat_Successful()
        {
            string updatedString = _manager.Update("REM v1.1.0");
            Assert.AreEqual("REM v1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithShortVersion_ShortFormat_Successful()
        {
            string updatedString = _manager.Update("REM v1.1");
            Assert.AreEqual("REM v1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithShortVersion_FullFormat_Space()
        {
            string updatedString = _manager.Update("REM v 1.1.0");
            Assert.AreEqual("REM v 1.1.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_FullFormat_Successful()
        {
            string updatedString = _manager.Update("REM version 1.1.0");
            Assert.AreEqual("REM version 1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_ShortFormat_Successful()
        {
            string updatedString = _manager.Update("REM version 1.1");
            Assert.AreEqual("REM version 1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_FullFormat_Space()
        {
            string updatedString = _manager.Update("REM version1.1.0");
            Assert.AreEqual("REM version1.1.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_LongFormat_Successful()
        {
            string updatedString = _manager.Update("REM v1.1.0 version 1.1.0");
            Assert.AreEqual("REM v1.2.0 version 1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_ShortFormat_Successful()
        {
            string updatedString = _manager.Update("REM v1.1 version 1.1");
            Assert.AreEqual("REM v1.2 version 1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_LongAndShortFormat_Successful()
        {
            string updatedString = _manager.Update("REM v1.1.0 version 1.1");
            Assert.AreEqual("REM v1.2.0 version 1.2", updatedString);
        }

        [TestMethod]
        public void Write_Successful()
        {
            _manager.IndexRecord = new MockIndexRecord {Thread = 99, Source = _source};
            _manager.Write(99, "REM 1.2.0");
            Assert.AreEqual(1, _manager.IndexRecords.Count);
            Assert.AreEqual(99, _manager.IndexRecords[0].Thread);
            Assert.AreEqual("InputFile.bat", _manager.IndexRecords[0].Source.RecordName);
            Assert.AreEqual("1.1.0", _manager.IndexRecords[0].Source.OldVersion);
            Assert.AreEqual("1.2.0", _manager.IndexRecords[0].Source.NewVersion);
        }

        [TestMethod]
        public void IsSourceAlreadyProcessed_Successful()
        {
            _manager.IndexRecord = new MockIndexRecord {Thread = 99, Source = _source};
            _manager.Write(99, "REM 1.2.0");
            Assert.IsTrue(_manager.IsSourceAlreadyProcessed(_source));
        }

        [TestMethod]
        public void Progress_Successful()
        {
            double processedCount = _manager.Progress(50);
            Assert.AreEqual(0.2, processedCount);
        }
    }
}