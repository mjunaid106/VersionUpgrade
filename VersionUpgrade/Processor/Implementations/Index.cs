using System.Collections.Generic;
using System.IO;
using System.Linq;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Index : IIndex
    {
        public string FileName { get; private set; }

        public Index(string fileName)
        {
            FileName = fileName;
            File.WriteAllText(fileName, "Thread, FileName, Old Version, New Version\n");
        }

        public void WriteIndex(IList<IIndexRecord> indexRecords)
        {
            File.AppendAllLines(FileName, indexRecords.Select(r => r.ToString()));
            //foreach (var indexRecord in indexRecords)
            //{
            //    File.AppendAllText(FileName,
            //   !string.IsNullOrEmpty(indexRecord.Source.OldVersion)
            //       ? string.Format("{0},{1},{2},{3}\n", indexRecord.Thread, indexRecord.Source.RecordName, indexRecord.Source.OldVersion, indexRecord.Source.NewVersion)
            //       : string.Format("{0},{1},N/A,N/A\n", indexRecord.Thread, indexRecord.Source.RecordName));
            //}
        }
    }
}
