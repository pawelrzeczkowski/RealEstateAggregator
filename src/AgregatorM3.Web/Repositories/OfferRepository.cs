using System;
using System.Collections.Generic;
using System.Linq;

namespace AgregatorM3.Web.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        public void AddToBlackList(string item)
        {
            System.IO.File.AppendAllText("blacklist.txt", item + Environment.NewLine);
        }

        public void AddToWhiteList(string item)
        {
            System.IO.File.AppendAllText("whitelist.txt", item + Environment.NewLine);
        }

        public List<string> GetBlackList()
        {
            var textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\blacklist.txt";
            var lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        public List<string> GetWhiteList()
        {
            var textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\whitelist.txt";
            var lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }
    }
}