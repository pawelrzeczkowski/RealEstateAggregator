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
    public class SearchController : Controller
    {
        // private static List<string> seenAdverts = ReadSeenData();
        private readonly ISingletonDataService _singletonDataService;
        private readonly IOfferRepository _offerRepository;

        public SearchController(ISingletonDataService singletonDataService, IOfferRepository offerRepository)
        {
            _singletonDataService = singletonDataService;
            _offerRepository = offerRepository;
        }

        public IActionResult Index()
        {
            return View(new List<string>());
        }

        public async Task<IActionResult> GetData(int priceMin, int priceMax)
        {
            // TODO USE ASYNC STREAMS
            // TODO https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
            // TODO https://stackoverflow.com/questions/46904678/call-signalr-core-hub-method-from-controller
            var resultList = await _singletonDataService.GetData(priceMin, priceMax);

            var blackList = _offerRepository.GetBlackList();
            var whiteList = _offerRepository.GetWhiteList();
            resultList = resultList.Except(blackList).Except(whiteList).Except(whiteList).Distinct().ToList();

            return View("Index", resultList);
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
            _offerRepository.AddToBlackList(item);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddToWhitelist(string item)
        {
            _offerRepository.AddToWhiteList(item);
            return Json(new { success = true });
        }
    }
}
