using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class GratkaScrappingService : IScrappingService
    {
        private readonly HttpClient client = new HttpClient();

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

                client.DefaultRequestHeaders.Add("Accept", "text/html");
                var response = await client.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length < url.Length - 10) break;
                if (!response.IsSuccessStatusCode) break;
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Nie znaleźliśmy tej strony")) break;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@class='row']/div[@class='container small-12 column']/div[@class='content']/div[@class='content__listingContainer']/div[@id='leftColumn']/article[@class='teaser ']/div[@class='teaser__content']/div[@class='teaser__infoBox']/h2/a");

                if (nodes == null)
                {
                    yield return new ResultModel("Gratka", "incorrect html select node query: Gratka");
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("Gratka", $"{node.GetAttributeValue("href", "incorrect htmlNode query: Gratka")}");
                }
            }
        }
    }
}
