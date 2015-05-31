using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Processor.Implementations;

namespace StartupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = 10000;
            const string filePath = "C:\\Files";
            var index = new Index(string.Format(@"{0}\index.csv", filePath));
            var indexWriterLockSlim = new ReaderWriterLockSlim();
            var sourceWriterLockSlim = new ReaderWriterLockSlim();
            var files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);
            foreach (string file in files.Where(f => !f.Contains("C:\\Files\\index.csv")))
            {
                Console.WriteLine("Processing file: {0}", file);
                var source = new Source(file);
                var manager = new Manager(source, index, indexWriterLockSlim, sourceWriterLockSlim);
                var originalData = manager.Read();
                var updatedData = manager.Update(originalData);
                var isSuccess = manager.Write(updatedData);
                Console.WriteLine("   Old Version: {0}", source.OldVersion);
                Console.WriteLine("   New Version: {0}", source.NewVersion);
                if (isSuccess)
                {
                    manager.UpdateIndex();
                }
            }
            Console.WriteLine("Total files processed: {0}", files.Count());
            Console.Read();

        }
    }
}
