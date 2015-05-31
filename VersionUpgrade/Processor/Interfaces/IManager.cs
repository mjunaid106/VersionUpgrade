namespace Processor.Interfaces
{
    public interface IManager
    {
        void UpdateIndex();
        string Read();
        string Update(string originalText);
        bool Write(string updatedText);
    }
}
