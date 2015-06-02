using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Processor.Implementations;
using Processor.Interfaces;

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
            var files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories).Where(f => !f.Contains("C:\\Files\\index.csv"));

            var tasks = new List<Task>();
            var manager = new Manager(index);
            for (int i = 0; i < 2; i++)
            {
                tasks.Add(Task.Run(() => Process(Task.CurrentId, files, manager)));
            }
            Console.WriteLine("Total files processed: {0}", files.Count());
            Console.Read();

        }

        private static void Process(int? thread, IEnumerable<string> files, IManager manager)
        {
            foreach (string file in files)
            {
                Console.WriteLine("Processing file: {0}", file);
                var source = new Source(file);
                // bool isProcessed = manager.IsSourceAlreadyProcessed(source);

                //if (!isProcessed)
                {
                    manager.Source = source;
                    var originalData = manager.Read();
                    var updatedData = manager.Update(originalData);
                    var isSuccess = manager.Write(thread.Value, updatedData);
                    Console.WriteLine("   Old Version: {0}", source.OldVersion);
                    Console.WriteLine("   New Version: {0}", source.NewVersion);
                    //if (isSuccess)
                    //{
                    //    manager.UpdateIndex(thread.Value);
                    //}
                }
            }
        }
    }
}
