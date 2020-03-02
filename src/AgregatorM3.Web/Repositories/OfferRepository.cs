using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AgregatorM3.Web.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private const string _blackListFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\blacklist.txt";
        private const string _whiteListFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\whitelist.txt";

        public void AddToBlackList(string item)
        {
            File.AppendAllText("blacklist.txt", item + Environment.NewLine);
        }

        public void AddToWhiteList(string item)
        {
            File.AppendAllText("whitelist.txt", item + Environment.NewLine);
        }

        public List<string> GetBlackList()
        {
            var textFile = _blackListFile;
            var lines = File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        public List<string> GetWhiteList()
        {
            var whiteListFile = _whiteListFile;
            var lines = File.ReadAllLines(whiteListFile);

            return lines.OfType<string>().ToList();
        }

        public void RemoveFromWhitelist(string item)
        {
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(_whiteListFile).Where(l => l != item);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(_whiteListFile);
            File.Move(tempFile, _whiteListFile);
        }
    }
}