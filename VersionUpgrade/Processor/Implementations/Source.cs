using System.IO;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Source : ISource
    {
        private readonly string _filePath;

        public string OldVersion { get; set; }
        public string NewVersion { get; set; }
        public string RecordName { get; set; }

        public Source(string filePath)
        {
            _filePath = filePath;
            RecordName = filePath;
        }

        public string ReadData()
        {
            return File.ReadAllText(_filePath);
        }

        public bool WriteData(string updatedText)
        {
            try
            {
                File.WriteAllText(_filePath, updatedText);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void Versions(string oldVersion, string newVersion)
        {
            OldVersion = oldVersion;
            NewVersion = newVersion;
        }
    }
}
