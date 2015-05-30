namespace Processor.Interfaces
{
    public interface ISource
    {
        string ReadData();

        bool WriteData(string updatedText);
    }
}
