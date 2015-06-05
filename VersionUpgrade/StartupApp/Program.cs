using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Processor.Implementations;
using Processor.Interfaces;

namespace StartupApp
{
    class Program
    {
        public static IEnumerable<string> Files { get; set; }
        static void Main(string[] args)
        {
            Console.BufferHeight = 10000;
            string filePath = args[0];
            int methodType = int.Parse(args[1]);
            var index = new Index(string.Format(@"{0}\index.csv", filePath));
            Files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories).Where(f => !f.Contains(index.FileName));

            var tasks = new List<Task>();
            IManager manager = new Manager(index);
            tasks.Add(Task.Run(() => Progress(manager)));

            var watch = new Stopwatch();
            watch.Start();
            if (methodType == 0)
            {
                manager.CheckIndexBeforeUpdate = true;
                for (int i = 0; i < 5; i++)
                {
                    tasks.Add(Task.Run(() => Process(Task.CurrentId.HasValue ? Task.CurrentId.Value : 0, Files, manager)));
                }
            }
            else
            {
                manager.CheckIndexBeforeUpdate = false;
                foreach (var file in Files)
                {
                    Task task = Task.Run(() => Process(Task.CurrentId.HasValue ? Task.CurrentId.Value : 0, file, manager));
                    tasks.Add(task);
                }
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Writing to index");
            manager.WriteIndex();
            Console.WriteLine("Total files processed: {0}", manager.IndexRecords.Count());
            Console.WriteLine("Check {0} for version information", index.FileName);
            Console.WriteLine("Press any key to exit..");
            watch.Stop();
            Console.WriteLine("Time to complete: {0}", watch.Elapsed.TotalSeconds);
            Console.Read();
        }

        private static void Progress(IManager manager)
        {
            double progress = 0;

            while (!progress.Equals(1.0))
            {
                progress = manager.Progress(Files.Count());
                Console.Clear();
                Console.WriteLine("{0} completed", progress.ToString("#0%"));
                Thread.Sleep(100);
            }
        }

        private static void Process(int thread, IEnumerable<string> files, IManager manager)
        {
            foreach (string file in files)
            {
                var source = new Source(file);

                bool isProcessed = manager.IsSourceAlreadyProcessed(source);

                if (!isProcessed)
                {
                    manager.Source = source;
                    manager.IndexRecord = new IndexRecord(thread, source);
                    var originalData = manager.Read();
                    var updatedData = manager.Update(originalData);
                    manager.Write(thread, updatedData);
                }

            }
        }

        private static void Process(int thread, string file, IManager manager)
        {
            var source = new Source(file);
            manager.Source = source;
            manager.IndexRecord = new IndexRecord(thread, source);
            var originalData = manager.Read();
            var updatedData = manager.Update(originalData);
            manager.Write(thread, updatedData);
        }
    }
}
