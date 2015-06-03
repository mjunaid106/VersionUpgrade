using System.Collections.Generic;
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

        public bool IsSourceProcessed(ISource source)
        {
            using (var file = new StreamReader(_fileName))
            {
                return file.ReadToEnd().Contains(source.RecordName);
            }
        }

        public void WriteIndex(IList<IIndexRecord> indexRecords)
        {
            foreach (var indexRecord in indexRecords)
            {
                File.AppendAllText(_fileName,
               !string.IsNullOrEmpty(indexRecord.Source.OldVersion)
                   ? string.Format("{0},{1},{2},{3}\n", indexRecord.Thread, indexRecord.Source.RecordName, indexRecord.Source.OldVersion, indexRecord.Source.NewVersion)
                   : string.Format("{0},{1},N/A,N/A\n", indexRecord.Thread, indexRecord.Source.RecordName));
            }
        }
    }
}
