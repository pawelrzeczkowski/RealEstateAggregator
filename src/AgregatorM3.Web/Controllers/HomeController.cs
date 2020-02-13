using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgregatorM3.Web.Services;

namespace AgregatorM3.Web.Controllers
{
    public class HomeController : Controller
    {
        // private static List<string> seenAdverts = ReadSeenData();
        private readonly IEnumerable<IScrappingService> _scrappingServices;

        public HomeController(IEnumerable<IScrappingService> scrappingServices)
        {
            _scrappingServices = scrappingServices;
        }

        public async Task<IActionResult> Index()
        {
            // USE ASYNC STREAMS
            var resultList = new List<string>();
            var priceMin = 500000;
            var priceMax = 950000;

            foreach (var service in _scrappingServices)
            {
                resultList.AddRange(await service.GetData(priceMin, priceMax));
            }

            var blackList = GetBlackList();
            var whiteList = GetWhiteList();
            resultList = resultList.Except(blackList).Except(whiteList).Distinct().ToList();

            return View(resultList);
        }

        private static List<string> GetBlackList()
        {
            var textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\blacklist.txt";
            var lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        private static List<string> GetWhiteList()
        {
            var textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\whitelist.txt";
            var lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        [HttpPost]
        public IActionResult Blacklist(string item)
        {
            System.IO.File.AppendAllText("blacklist.txt", item + Environment.NewLine);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Whitelist(string item)
        {
            System.IO.File.AppendAllText("whitelist.txt", item + Environment.NewLine);

            return Json(new { success = true });
        }
    }
}
