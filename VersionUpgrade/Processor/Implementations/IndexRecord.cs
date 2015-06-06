using Processor.Interfaces;

namespace Processor.Implementations
{
    public class IndexRecord : IIndexRecord
    {
        public IndexRecord(int threadId, ISource source)
        {
            Thread = threadId;
            Source = source;
        }

        public int Thread { get; set; }
        public ISource Source { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Source.OldVersion)
                ? string.Format("{0},{1},{2},{3}", Thread, Source.RecordName, Source.OldVersion, Source.NewVersion)
                : string.Format("{0},{1},N/A,N/A", Thread, Source.RecordName);
        }
    }
}