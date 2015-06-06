using System.Collections.Generic;
using System.IO;
using System.Linq;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Index : IIndex
    {
        public Index(string fileName)
        {
            FileName = fileName;
            File.WriteAllText(fileName, "Thread, FileName, Old Version, New Version\n");
        }

        public string FileName { get; private set; }

        public void WriteIndex(IList<IIndexRecord> indexRecords)
        {
            File.AppendAllLines(FileName, indexRecords.Select(r => r.ToString()));
        }
    }
}