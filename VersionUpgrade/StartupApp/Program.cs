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
        public static IEnumerable<string> Files { get; set; }
        static void Main(string[] args)
        {
            Console.BufferHeight = 10000;
            //const string filePath = "C:\\Files";
            string filePath = args[0];
            var index = new Index(string.Format(@"{0}\index.csv", filePath));
            Files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories).Where(f => !f.Contains("C:\\Files\\index.csv"));

            var tasks = new List<Task>();
            var manager = new Manager(index);
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(() => Process(Task.CurrentId, Files, manager)));
            }
            tasks.Add(Task.Run(() => Progress(manager)));
            Task.WaitAll(tasks.ToArray());
            manager.WriteIndex();
            Console.WriteLine("Total files processed: {0}", Files.Count());
            Console.Read();

        }

        private static void Progress(Manager manager)
        {
            double progress = 0;

            while (!progress.Equals(1.0))
            {
                progress = manager.Progress(Files.Count());
                Console.Clear();
                Console.WriteLine("{0} completed", progress.ToString("##%"));
                Thread.Sleep(1000);
            }

        }

        private static void Process(int? thread, IEnumerable<string> files, IManager manager)
        {
            foreach (string file in files)
            {
                var source = new Source(file);
                bool isProcessed = manager.IsSourceAlreadyProcessed(source);

                if (!isProcessed)
                {
                    manager.Source = source;
                    var originalData = manager.Read();
                    var updatedData = manager.Update(originalData);
                    manager.Write(thread.HasValue ? thread.Value : 0, updatedData);
                }

            }
        }
    }
}
