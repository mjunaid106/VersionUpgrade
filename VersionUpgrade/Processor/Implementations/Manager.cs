using System;
using System.Text.RegularExpressions;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Manager : IManager
    {
        public ISource Source { get; set; }
        private IIndex Index;
        private static ReaderWriterLockSlim _sourceReadWriteLock;
        private static ReaderWriterLockSlim _indexReadWriteLock;

        public Manager(IIndex index)
        {
            Index = index;
            _sourceReadWriteLock = new ReaderWriterLockSlim();
            _indexReadWriteLock = new ReaderWriterLockSlim();
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

        public bool Write(int threadId, string updatedText)
        {
            //try
            //{
            _indexReadWriteLock.EnterWriteLock();
            _sourceReadWriteLock.EnterWriteLock();
            try
            {
                Source.WriteData(updatedText);
                if (!IsSourceAlreadyProcessed(Source))
                {
                    Index.Update(threadId, Source);
                }
            }
            finally
            {
                _indexReadWriteLock.ExitWriteLock();
                _sourceReadWriteLock.ExitWriteLock();
            }
            return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        public bool IsSourceAlreadyProcessed(ISource source)
        {
            bool isProcessed;

            //_indexReadWriteLock.EnterReadLock();

            try
            {
                isProcessed = Index.IsSourceProcessed(source);
            }
            finally
            {
                //_indexReadWriteLock.ExitReadLock();
            }
            return isProcessed;
        }

        //public void UpdateIndex(int thread)
        //{
        //    _indexReadWriteLock.EnterWriteLock();
        //    Index.Update(thread, Source);
        //    _indexReadWriteLock.ExitWriteLock();
        //    _indexReadWriteLock.ExitReadLock();
        //}

    }
}
