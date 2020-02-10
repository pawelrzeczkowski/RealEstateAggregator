using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using HtmlAgilityPack;
using AgregatorM3.Web.Services;

namespace AgregatorM3.Web.Controllers
{
    public class HomeController : Controller
    {
       // private static HttpClient client = new HttpClient();
       // private static List<string> seenAdverts = ReadSeenData();
        private readonly IScrappingService _domImportaService;

        public HomeController(IScrappingService scrappingService)
        {
            _domImportaService = scrappingService;
        }

        public async Task<IActionResult> Index()
        {
            // USE ASYNC STREAMS

            var priceMin = 500000;
            var priceMax = 950000;

            var result = await _domImportaService.GetData(priceMin, priceMax);
               // Gumtree(priceMin, priceMax, seenAdverts);

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
