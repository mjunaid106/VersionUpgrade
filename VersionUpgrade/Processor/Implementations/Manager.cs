using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Manager : IManager, IDisposable
    {
        public ISource Source { get; set; }
        private IIndex Index;
        private static ReaderWriterLockSlim _sourceReadWriteLock;

        public static List<IIndexRecord> IndexRecords { get; set; }

        public Manager(IIndex index)
        {
            Index = index;
            _sourceReadWriteLock = new ReaderWriterLockSlim();
            IndexRecords = new List<IIndexRecord>();
        }

        public string Read()
        {
            string sourceData;

            _sourceReadWriteLock.EnterReadLock();
            try
            {
                sourceData = Source.ReadData();
            }
            finally
            {
                _sourceReadWriteLock.ExitReadLock();
            }
            return sourceData;
        }

        public string Update(string originalText)
        {
            const string pattern1 = @"v\d+.\d+(.\d+)*";
            const string pattern2 = @"version \d+.\d+(.\d+)*";
            string updatedString = Regex.Replace(originalText, pattern1, Incrementor, RegexOptions.IgnoreCase);
            updatedString = Regex.Replace(updatedString, pattern2, Incrementor, RegexOptions.IgnoreCase);
            return updatedString;
        }

        private string Incrementor(Match match)
        {
            string[] version = match.Value.Split('.');
            version[1] = (Convert.ToInt32(version[1]) + 1).ToString();
            string updatedVersions = string.Join(".", version);

            Source.Versions(match.Value, updatedVersions);

            return string.Join(".", version);
        }

        public void Write(int threadId, string updatedText)
        {
            _sourceReadWriteLock.EnterWriteLock();
            try
            {
                Source.WriteData(updatedText);
                if (!IsSourceAlreadyProcessed(Source))
                {
                    IndexRecords.Add(new IndexRecord(threadId, Source));
                }
            }
            finally
            {
                _sourceReadWriteLock.ExitWriteLock();
            }
        }

        public bool IsSourceAlreadyProcessed(ISource source)
        {
            bool isProcessed;
            lock (IndexRecords)
            {
                isProcessed = IndexRecords.ToList().Any(s => s.Source.RecordName.Equals(source.RecordName));
            }
            return isProcessed;
        }

        public void WriteIndex()
        {
            Index.WriteIndex(IndexRecords);
        }

        public double Progress(int fileCount)
        {
            double percentage;
            lock (IndexRecords)
            {
                percentage = Convert.ToDouble(IndexRecords.Count()) / Convert.ToDouble(fileCount);
            }
            return percentage;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
