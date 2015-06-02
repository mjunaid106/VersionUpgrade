namespace Processor.Interfaces
{
    public interface IManager
    {
        ISource Source { get; set; }

        //void UpdateIndex(int thread);
        string Read();
        string Update(string originalText);
        bool Write(string updatedText);
        bool IsSourceAlreadyProcessed(ISource source);
    }
}
