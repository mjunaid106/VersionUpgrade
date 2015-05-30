using System.IO;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Source : ISource
    {
        private readonly string _filePath;

        public Source(string filePath)
        {
            _filePath = filePath;
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
    }
}
