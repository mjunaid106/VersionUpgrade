using System.Collections.Generic;

namespace Processor.Interfaces
{
    public interface IManager
    {
        ISource Source { get; set; }
        IList<IIndexRecord> IndexRecords { get; set; }

        string Read();
        string Update(string originalText);
        void Write(int threadId, string updatedText);
        bool IsSourceAlreadyProcessed(ISource source);
        void WriteIndex();

        double Progress(int fileCount);
    }
}
