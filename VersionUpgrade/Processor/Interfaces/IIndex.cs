namespace Processor.Interfaces
{
    public interface IIndex
    {
        void Update(ISource source);
        bool IsSourceProcessed(ISource source);
    }
}
