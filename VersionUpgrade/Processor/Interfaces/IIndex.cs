namespace Processor.Interfaces
{
    public interface IIndex
    {
        void Update(int thread, ISource source);
        bool IsSourceProcessed(ISource source);
    }
}
