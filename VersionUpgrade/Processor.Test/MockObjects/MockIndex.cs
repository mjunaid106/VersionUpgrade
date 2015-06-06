using System.Collections.Generic;
using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockIndex : IIndex
    {
        public MockIndex(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }

        public void WriteIndex(IList<IIndexRecord> indexRecords)
        {
        }
    }
}