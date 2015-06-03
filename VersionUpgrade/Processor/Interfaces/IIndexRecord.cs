namespace Processor.Interfaces
{
    public interface IIndexRecord
    {
        int Thread { get; set; }
        ISource Source { get; set; }
    }
}
