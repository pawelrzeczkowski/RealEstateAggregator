using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Exceptionless;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class OlxScrappingService : IScrappingService
    {
        private readonly HttpClient _httpClient;

        public OlxScrappingService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            for (var page = 1; page < 7; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"&page={page}";
                var referer = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/";
                var url = $"{referer}?search%5Bfilter_float_price%3Afrom%5D="
                    + $"{searchModel.PriceFrom}&search%5Bfilter_float_price%3Ato%5D={searchModel.PriceTo}&search%5Bfilter_enum_market%5D%5B0%5D=secondary&search%5B"
                    + "filter_enum_rooms%5D%5B0%5D=three&search%5Bfilter_enum_rooms%5D%5B1%5D=five"
                    + "&search%5Bdistrict_id%5D=353" + paging;

                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,pl;q=0.8");
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                Thread.Sleep(200);

                var response = await _httpClient.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length < url.Length - 20) break;
                if (!response.IsSuccessStatusCode)
                {
                    ExceptionlessClient.Default.SubmitLog("response status code not 200: " + response.Content.ReadAsStringAsync());
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Nie znaleźliśmy ogłoszeń dla tego zapytania.")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='innerLayout']/div[@id='listContainer']/section[@id='body-container']/div[3]/div/div[1]/table[@id='offers_table']/tbody/tr/td/div/table/tbody/tr/td[2]/div/h3/a");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Olx, htmlContent: " + content);
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("Olx", $"{node.GetAttributeValue("href", "incorrect htmlNode query: Olx")}");
                }
            }
        }
    }
}
