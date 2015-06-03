using Processor.Interfaces;

namespace Processor.Implementations
{
    public class IndexRecord: IIndexRecord
    {
      
        public IndexRecord(int threadId, ISource source)
        {
            Thread = threadId;
            Source = source;
        }
        public int Thread { get; set; }
        public ISource Source { get; set; }
    }
}
