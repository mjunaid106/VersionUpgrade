using System;
using System.Collections.Generic;
using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockManager : IManager
    {
        public ISource Source { get; set; }
        public IIndexRecord IndexRecord { get; set; }
        public IList<IIndexRecord> IndexRecords { get; set; }
        public bool CheckIndexBeforeUpdate { get; set; }

        public string Read()
        {
            throw new NotImplementedException();
        }

        public string Update(string originalText)
        {
            throw new NotImplementedException();
        }

        public void Write(int threadId, string updatedText)
        {
            throw new NotImplementedException();
        }

        public bool IsSourceAlreadyProcessed(ISource source)
        {
            throw new NotImplementedException();
        }

        public void WriteIndex()
        {
            throw new NotImplementedException();
        }

        public double Progress(int fileCount)
        {
            throw new NotImplementedException();
        }
    }
}
