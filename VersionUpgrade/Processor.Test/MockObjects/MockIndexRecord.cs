using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockIndexRecord : IIndexRecord
    {
        public int Thread { get; set; }
        public ISource Source { get; set; }
    }
}
