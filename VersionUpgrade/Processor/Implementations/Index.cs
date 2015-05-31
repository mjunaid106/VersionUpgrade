using System.IO;
using System.Linq;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Index : IIndex
    {
        private readonly string _fileName;
        private static readonly object SyncRoot = new object();
        public Index(string fileName)
        {
            _fileName = fileName;
            //lock (SyncRoot)
            //{
               // File.WriteAllText(fileName, "FileName, Old Version, New Version\n");
            //}
        }

        public void Update(ISource source)
        {
            //lock (SyncRoot)
            {
                File.AppendAllText(_fileName,
                    !string.IsNullOrEmpty(source.OldVersion)
                        ? string.Format("{0},{1},{2}\n", source.RecordName, source.OldVersion, source.NewVersion)
                        : string.Format("{0},N/A,N/A\n", source.RecordName));
            }
        }

        public bool IsSourceProcessed(ISource source)
        {
            bool isProccesed = false;
            using (var file = new StreamReader(_fileName))
            {
                isProccesed = file.ReadToEnd().Contains(source.RecordName);   
            }
            return isProccesed;
        }
    }
}
