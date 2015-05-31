namespace Processor.Interfaces
{
    public interface ISource
    {
        string OldVersion { get; set; }
        string NewVersion { get; set; }
        string RecordName { get; set; }

        string ReadData();

        bool WriteData(string updatedText);

        void Versions(string oldVersion, string newVersion);
    }
}
