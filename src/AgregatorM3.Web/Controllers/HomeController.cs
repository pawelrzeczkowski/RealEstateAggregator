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
            var result = new List<string>();
            var priceMin = 500000;
            var priceMax = 950000;

            foreach (var service in _scrappingServices)
            {
                result.AddRange(await service.GetData(priceMin, priceMax));
            }

            return View(result);
        }

        private static List<string> ReadSeenData()
        {
            var textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.Web\blacklist.txt";
            var lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        [HttpPost]
        public IActionResult AddToBlacklist([FromBody]string item)
        {
            System.IO.File.AppendAllText("blacklist.txt", item + Environment.NewLine);

            return Json(new { success = true });
        }
    }
}
