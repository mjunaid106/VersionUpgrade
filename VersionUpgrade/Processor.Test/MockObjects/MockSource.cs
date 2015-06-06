using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockSource : ISource
    {
        public string OldVersion { get; set; }
        public string NewVersion { get; set; }
        public string RecordName { get; set; }

        public string ReadData()
        {
            return "REM v1.1.0";
        }

        public bool WriteData(string updatedText)
        {
            return updatedText == "REM v1.2.0";
        }

        public void Versions(string oldVersion, string newVersion)
        {
            OldVersion = oldVersion;
            NewVersion = newVersion;
        }
    }
}