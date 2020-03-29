using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Exceptionless;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class MorizonScrappingService : IScrappingService
    {
        private readonly HttpClient _httpClient;

        public MorizonScrappingService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            for (var page = 1; page < 7; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"&page={page}";

                var referer = "https://www.morizon.pl/mieszkania/warszawa/mokotow/";
                var url =
                    $"{referer}?ps%5Bprice_from%5D={searchModel.PriceFrom}&ps%5Bprice_to%5D={searchModel.PriceTo}&ps%5Bprice_m2_to%5D={searchModel.PricePerMeterTo}"
                    + $"&ps%5Bliving_area_from%5D={searchModel.SurfaceFrom}&ps%5Bliving_area_to%5D={searchModel.SurfaceTo}&ps%5Bnumber_of_rooms_from%5D=" 
                    + $"{searchModel.RoomsFrom}&ps%5Bnumber_of_rooms_to%5D={searchModel.RoomsTo}&ps%5Bhas_parking_places%5D=1&ps%5Bmarket_type%5D%5B0%5D=2" + paging;

                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,pl;q=0.8");
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                Thread.Sleep(50);

                var response = await _httpClient.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length != url.Length) break;
                if (!response.IsSuccessStatusCode)
                {
                    ExceptionlessClient.Default.SubmitLog("response status code not 200: " + response.Content.ReadAsStringAsync());
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='background']/div[@id='contentPage']/div[@class='contentBox']/div/div/div/section/div/div/div/div/section/header/a");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Morizon, htmlContent: " + content);
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("Morizon", $"{node.GetAttributeValue("href", "incorrect htmlNode query: Morizon")}");
                }
            }
        }
    }
}
