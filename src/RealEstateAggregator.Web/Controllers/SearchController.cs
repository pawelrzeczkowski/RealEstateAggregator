using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateAggregator.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using RealEstateAggregator.Web.Services;
using RealEstateAggregator.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Controllers
{
    public class SearchController : Controller
    {
        // private static List<string> seenAdverts = ReadSeenData();
        private readonly ISingletonDataService _singletonDataService;
        private readonly IOfferRepository _offerRepository;
        private readonly IHubContext<DynamicResultsHub> _signalHub;

        public SearchController(ISingletonDataService singletonDataService, IOfferRepository offerRepository, IHubContext<DynamicResultsHub> hub)
        {
            _singletonDataService = singletonDataService;
            _offerRepository = offerRepository;
            _signalHub = hub;
        }

        public IActionResult Index()
        {
            return View(new SearchModel());
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
        public async Task<IActionResult> GetData(SearchModel parameters)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio

            var resultCounter = 0;
            var blackList = _offerRepository.GetBlackList();
            var whiteList = _offerRepository.GetWhiteList();

            await foreach (var result in _singletonDataService.GetData(parameters))
            {
                await _signalHub.Clients.All.SendAsync("DisplayProgress", result.ServiceName);

                if (!blackList.Contains(result.OfferLink) && !whiteList.Contains(result.OfferLink))
                {
                    resultCounter++;
                    await _signalHub.Clients.All.SendAsync("DisplayOfferLink", resultCounter, result.OfferLink);
                }
            }

            return Json("done");
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

        [HttpPost]
        public IActionResult RemoveFromWhitelist(string item)
        {
            _offerRepository.RemoveFromWhitelist(item);
            return Json(new { success = true });
        }
    }
}
