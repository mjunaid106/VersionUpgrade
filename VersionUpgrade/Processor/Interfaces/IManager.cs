namespace Processor.Interfaces
{
    public interface IManager
    {
        void ReserveIndex();
        void ReleaseIndex();
        string Read();
        string Update(string originalText);
        void Write(string updatedText);
    }
}
