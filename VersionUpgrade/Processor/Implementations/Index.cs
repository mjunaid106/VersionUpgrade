using System.IO;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Index : IIndex
    {
        private readonly string _fileName;

        public Index(string fileName)
        {
            _fileName = fileName;
            File.WriteAllText(fileName, "FileName, Old Version, New Version\n");
        }

        public void Update(ISource source)
        {
            File.AppendAllText(_fileName,
                !string.IsNullOrEmpty(source.OldVersion)
                    ? string.Format("{0},{1},{2}\n", source.RecordName, source.OldVersion, source.NewVersion)
                    : string.Format("{0},N/A,N/A\n", source.RecordName));
        }
    }
}
