using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Manager : IManager
    {
        private static ReaderWriterLockSlim _sourceReadWriteLock;
        private readonly IIndex _index;

        public Manager(IIndex index)
        {
            _index = index;
            _sourceReadWriteLock = new ReaderWriterLockSlim();
            IndexRecords = new List<IIndexRecord>();
        }

        public ISource Source { get; set; }
        public IIndexRecord IndexRecord { get; set; }

        public IList<IIndexRecord> IndexRecords { get; private set; }
        public bool CheckIndexBeforeUpdate { get; set; }

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

        public void Write(int threadId, string updatedText)
        {
            _sourceReadWriteLock.EnterWriteLock();
            try
            {
                // Extra check so that the file is not picked up by other threads.
                if (CheckIndexBeforeUpdate && !IsSourceAlreadyProcessed(Source))
                {
                    Source.WriteData(updatedText);
                    IndexRecords.Add(IndexRecord);
                }
                else if (!CheckIndexBeforeUpdate)
                {
                    Source.WriteData(updatedText);
                    IndexRecords.Add(IndexRecord);
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
                List<IIndexRecord> indexAsList = IndexRecords.ToList();
                isProcessed = indexAsList.Any(s => s.Source.RecordName.Equals(source.RecordName));
            }
            return isProcessed;
        }

        public void WriteIndex()
        {
            _index.WriteIndex(IndexRecords);
        }

        public double Progress(int fileCount)
        {
            double percentage;
            lock (IndexRecords)
            {
                percentage = Convert.ToDouble(IndexRecords.Count())/Convert.ToDouble(fileCount);
            }
            return percentage;
        }

        private string Incrementor(Match match)
        {
            string[] version = match.Value.Split('.');
            version[1] = (Convert.ToInt32(version[1]) + 1).ToString();
            string updatedVersions = string.Join(".", version);

            Source.Versions(match.Value, updatedVersions);

            return string.Join(".", version);
        }
    }
}