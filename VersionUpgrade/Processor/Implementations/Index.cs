using System.IO;
using System.Linq;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Index : IIndex
    {
        private readonly string _fileName;
       

        public Index(string fileName)
        {
            _fileName = fileName;

            File.WriteAllText(fileName, "Thread, FileName, Old Version, New Version\n");
        }

        public void Update(int thread, ISource source)
        {
            File.AppendAllText(_fileName,
                !string.IsNullOrEmpty(source.OldVersion)
                    ? string.Format("{0},{1},{2},{3}\n", thread, source.RecordName, source.OldVersion, source.NewVersion)
                    : string.Format("{0},{1},N/A,N/A\n", thread, source.RecordName));
        }

        public bool IsSourceProcessed(ISource source)
        {
            using (var file = new StreamReader(_fileName))
            {
                return file.ReadToEnd().Contains(source.RecordName);
            }
        }
    }
}
