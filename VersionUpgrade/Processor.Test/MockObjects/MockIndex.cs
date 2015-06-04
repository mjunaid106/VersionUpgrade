using System;
using System.Collections.Generic;
using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockIndex : IIndex
    {
        public void Update(int thread, ISource source)
        {
            throw new NotImplementedException();
        }

        public string FileName { get; private set; }

        public bool IsSourceProcessed(ISource source)
        {
            throw new NotImplementedException();
        }

        public void WriteIndex(IList<IIndexRecord> indexRecords)
        {
            throw new NotImplementedException();
        }


        public void RecordAlreadyProcessed(int thread, ISource source)
        {
            throw new NotImplementedException();
        }
    }
}
