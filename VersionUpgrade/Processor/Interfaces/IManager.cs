namespace Processor.Interfaces
{
    public interface IManager
    {
        IIndex Index { get; set; }
        ISource Source { get; set; }
        void UpdateIndex();
        string Read();
        string Update(string originalText);
        bool Write(string updatedText);
    }
}
