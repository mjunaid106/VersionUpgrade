using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockManager : IManager
    {
        private readonly IIndex _index;

        public MockManager(IIndex index)
        {
            _index = index;
            IndexRecords = new List<IIndexRecord>();
        }

        public ISource Source { get; set; }
        public IIndexRecord IndexRecord { get; set; }
        public IList<IIndexRecord> IndexRecords { get; private set; }
        public bool CheckIndexBeforeUpdate { get; set; }

        public string Read()
        {
            throw new NotImplementedException();
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
            IndexRecords.Add(IndexRecord);
        }

        public bool IsSourceAlreadyProcessed(ISource source)
        {
            return IndexRecords.ToList().Any(s => s.Source.RecordName.Equals(source.RecordName));
        }

        public void WriteIndex()
        {
        }

        public double Progress(int fileCount)
        {
            return 10.0/fileCount;
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