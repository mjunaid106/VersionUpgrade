using System.Collections.Generic;

namespace Processor.Interfaces
{
    public interface IIndex
    {
        string FileName { get; }

        void WriteIndex(IList<IIndexRecord> indexRecords);
    }
}