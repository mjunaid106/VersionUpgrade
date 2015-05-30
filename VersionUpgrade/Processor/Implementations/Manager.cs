using System;
using System.Text.RegularExpressions;
using System.Threading;
using Processor.Interfaces;

namespace Processor.Implementations
{
    public class Manager : IManager
    {
        private readonly ISource _source;

        public Manager(ISource source)
        {
            _source = source;
        }

        public string Read()
        {
            return _source.ReadData();
        }

        public string Update(string originalText)
        {
            const string pattern1 = @"v\d+.\d+(.\d+)*";
            const string pattern2 = @"version \d+.\d+(.\d+)*";
            var updatedString = Regex.Replace(originalText, pattern1, Incrementor, RegexOptions.Multiline);
            updatedString = Regex.Replace(updatedString, pattern2, Incrementor, RegexOptions.Multiline);
            return updatedString;
        }

        private string Incrementor(Match match)
        {
            string[] version = match.Value.Split('.');
            version[1] = (Convert.ToInt32(version[1]) + 1).ToString();
            return string.Join(".", version);
        }

        public void Write(string updatedText)
        {
            _source.WriteData(updatedText);
        }

        public void ReserveIndex()
        {
            throw new NotImplementedException();
        }

        public void ReleaseIndex()
        {
            throw new NotImplementedException();
        }
    }
}
