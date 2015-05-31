using System;
using System.Text.RegularExpressions;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Manager : IManager
    {
        private readonly ISource _source;
        private readonly IIndex _index;
        private static ReaderWriterLockSlim _indexReadWriteLock;
        private static ReaderWriterLockSlim _sourceReadWriteLock;

        public Manager(ISource source, IIndex index, ReaderWriterLockSlim indexReadWriteLock, ReaderWriterLockSlim sourceReadWriteLock)
        {
            _source = source;
            _index = index;
            _indexReadWriteLock = indexReadWriteLock;
            _sourceReadWriteLock = sourceReadWriteLock;
        }

        public string Read()
        {
            string sourceData;

            _sourceReadWriteLock.EnterReadLock();
            try
            {
                sourceData = _source.ReadData();
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

            _source.Versions(match.Value, updatedVersions);

            return string.Join(".", version);
        }

        public bool Write(string updatedText)
        {
            try
            {
                _sourceReadWriteLock.EnterWriteLock();
                try
                {
                    _source.WriteData(updatedText);
                }
                finally
                {
                    _sourceReadWriteLock.ExitWriteLock();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateIndex()
        {
            _indexReadWriteLock.EnterWriteLock();
            try
            {
                _index.Update(_source);
            }
            finally
            {
                _indexReadWriteLock.ExitWriteLock();
            }
        }
    }
}
