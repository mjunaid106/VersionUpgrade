using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor.Implementations;
using Processor.Interfaces;
using Processor.Test.MockObjects;

namespace Processor.Test
{
    [TestClass]
    public class ProcessorTests
    {
        private ISource _source;
        private IManager _manager;

        [TestInitialize]
        public void Initialise()
        {
            _source = new MockSource();
            _manager = new Manager(_source);
        }

        [TestMethod]
        public void Update_WithShortVersion_FullFormat_Successful()
        {
            var updatedString = _manager.Update("REM v1.1.0");
            Assert.AreEqual("REM v1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithShortVersion_ShortFormat_Successful()
        {
            var updatedString = _manager.Update("REM v1.1");
            Assert.AreEqual("REM v1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithShortVersion_FullFormat_Space()
        {
            var updatedString = _manager.Update("REM v 1.1.0");
            Assert.AreEqual("REM v 1.1.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_FullFormat_Successful()
        {
            var updatedString = _manager.Update("REM version 1.1.0");
            Assert.AreEqual("REM version 1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_ShortFormat_Successful()
        {
            var updatedString = _manager.Update("REM version 1.1");
            Assert.AreEqual("REM version 1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithLongVersion_FullFormat_Space()
        {
            var updatedString = _manager.Update("REM version1.1.0");
            Assert.AreEqual("REM version1.1.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_LongFormat_Successful()
        {
            var updatedString = _manager.Update("REM v1.1.0 version 1.1.0");
            Assert.AreEqual("REM v1.2.0 version 1.2.0", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_ShortFormat_Successful()
        {
            var updatedString = _manager.Update("REM v1.1 version 1.1");
            Assert.AreEqual("REM v1.2 version 1.2", updatedString);
        }

        [TestMethod]
        public void Update_WithLongAndShortVersion_LongAndShortFormat_Successful()
        {
            var updatedString = _manager.Update("REM v1.1.0 version 1.1");
            Assert.AreEqual("REM v1.2.0 version 1.2", updatedString);
        }
    }
}
