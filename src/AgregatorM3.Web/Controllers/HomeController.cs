using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AgregatorM3.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using AgregatorM3.Web.Services;

namespace AgregatorM3.Web.Controllers
{
    public class HomeController : Controller
    {
        // private static List<string> seenAdverts = ReadSeenData();
        private readonly IEnumerable<IScrappingService> _scrappingServices;
        private readonly IOfferRepository _offerRepository; 

        public HomeController(IEnumerable<IScrappingService> scrappingServices, IOfferRepository offerRepository)
        {
            _scrappingServices = scrappingServices;
            _offerRepository = offerRepository;
        }

        public async Task<IActionResult> Index()
        {
            // USE ASYNC STREAMS
            var resultList = new List<string>();
            var priceMin = 500000;
            var priceMax = 950000;

            foreach (var service in _scrappingServices)
            {
                var singleResult = await service.GetData(priceMin, priceMax);
                resultList.AddRange(singleResult);
            }

            var blackList = _offerRepository.GetBlackList();
            var whiteList = _offerRepository.GetWhiteList();
            resultList = resultList.Except(blackList).Except(whiteList).Except(whiteList).Distinct().ToList();

            return View(resultList);
        }

        public IActionResult Whitelist()
        {
            return View(_offerRepository.GetWhiteList());
        }

        public IActionResult Blacklist()
        {
            return View(_offerRepository.GetBlackList());
        }

        [HttpPost]
        public IActionResult AddToBlacklist(string item)
        {
            System.IO.File.AppendAllText("blacklist.txt", item + Environment.NewLine);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddToWhitelist(string item)
        {
            System.IO.File.AppendAllText("whitelist.txt", item + Environment.NewLine);

            return Json(new { success = true });
        }
    }
}
