using System.Collections.Generic;

namespace Processor.Interfaces
{
    public interface IIndex
    {
        string FileName { get; }

        bool IsSourceProcessed(ISource source);

        void WriteIndex(IList<IIndexRecord> indexRecords);
    }
}
