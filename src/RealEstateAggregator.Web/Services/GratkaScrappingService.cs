using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Exceptionless;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class GratkaScrappingService : IScrappingService
    {
        private readonly HttpClient _httpClient;

        public GratkaScrappingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            for (var page = 1; page < 10; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"page={page}&";
                var url =
                    $"https://gratka.pl/nieruchomosci/mieszkania/warszawa/mokotow/sprzedaz?{paging}cena-calkowita:min={searchModel.PriceFrom}&cena-calkowita:max={searchModel.PriceTo}"
                    + $"&rynek=wtorny&cena-za-m2:max={searchModel.PricePerMeterTo}&powierzchnia-w-m2:min={searchModel.SurfaceFrom}&powierzchnia-w-m2:max={searchModel.SurfaceTo}" 
                    + $"&liczba-pokoi:min={searchModel.RoomsFrom}&liczba-pokoi:max={searchModel.RoomsTo}&pietro:min=3&pietro:max=999";

                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,pl;q=0.8");
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                Thread.Sleep(200);

                var response = await _httpClient.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length < url.Length - 10) break;
                if (!response.IsSuccessStatusCode)
                {
                    ExceptionlessClient.Default.SubmitLog("response status code not 200: " + response.Content.ReadAsStringAsync());
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Nie znaleźliśmy tej strony")) break;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@class='row']/div[@class='container small-12 column']/div[@class='content']/div[@class='content__listingContainer']/div[@id='leftColumn']/article[@class='teaser ']/div[@class='teaser__content']/div[@class='teaser__infoBox']/h2/a");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Gratka, htmlContent: " + content);
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("Gratka", $"{node.GetAttributeValue("href", "incorrect htmlNode query: Gratka")}");
                }
            }
        }
    }
}
