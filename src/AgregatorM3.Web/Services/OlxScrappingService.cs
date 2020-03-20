using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AgregatorM3.Web.Models;
using HtmlAgilityPack;

namespace AgregatorM3.Web.Services
{
    public class OlxScrappingService : IScrappingService
    {
        private readonly HttpClient client = new HttpClient();

        public async IAsyncEnumerable<string> GetData(SearchModel searchModel)
        {
            for (var page = 1; page < 7; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"&page={page}";
                var url =
                    $"https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/?search%5Bfilter_float_price%3Afrom%5D="
                    + $"{searchModel.PriceFrom}&search%5Bfilter_float_price%3Ato%5D={searchModel.PriceTo}&search%5Bfilter_enum_market%5D%5B0%5D=secondary&search%5B"
                    + "filter_enum_rooms%5D%5B0%5D=three&search%5Bfilter_enum_rooms%5D%5B1%5D=five"
                    + "&search%5Bdistrict_id%5D=353" + paging;

                client.DefaultRequestHeaders.Add("Accept", "text/html");
                var response = await client.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length < url.Length - 20) break;
                if (!response.IsSuccessStatusCode) break;
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Nie znaleźliśmy ogłoszeń dla tego zapytania.")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='innerLayout']/div[@id='listContainer']/section[@id='body-container']/div[3]/div/div[1]/table[@id='offers_table']/tbody/tr/td/div/table/tbody/tr/td[2]/div/h3/a");

                foreach (var node in nodes)
                {
                    yield return $"{node.GetAttributeValue("href", "incorrect htmlNode query")}";
                }
            }
        }
    }
}
