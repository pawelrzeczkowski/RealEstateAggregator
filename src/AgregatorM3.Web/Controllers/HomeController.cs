using System.Collections.Generic;
using System.Linq;
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
            string textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.ConsoleApp\seen.txt";
            string[] lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }
    }
}
