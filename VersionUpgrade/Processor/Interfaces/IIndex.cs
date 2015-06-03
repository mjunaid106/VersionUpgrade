using System.Collections.Generic;

namespace Processor.Interfaces
{
    public interface IIndex
    {
        //void Update(int thread, ISource source);
        bool IsSourceProcessed(ISource source);

        void WriteIndex(IList<IIndexRecord> indexRecords);
    }
}
