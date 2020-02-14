using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace AgregatorM3.Web.Services
{
    public class MorizonScrappingService : IScrappingService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<List<string>> GetData(int priceMin, int priceMax)
        {
            var resultsList = new List<string>();
            for (var page = 1; page < 7; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"&page={page}";
                var url =
                    $"https://www.morizon.pl/mieszkania/warszawa/mokotow/?ps%5Bprice_from%5D={priceMin}&ps%5Bprice_to%5D={priceMax}&ps%5Bprice_m2_to%5D=13500"
                    + "&ps%5Bliving_area_from%5D=60&ps%5Bliving_area_to%5D=100&ps%5Bnumber_of_rooms_from%5D=3&ps%5Bnumber_of_rooms_to%5D=4&ps%5Bhas_parking_places%"
                    + "5D=1&ps%5Bmarket_type%5D%5B0%5D=2" + paging;

                client.DefaultRequestHeaders.Add("Accept", "text/html");
                var response = await client.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length != url.Length) break;
                if (!response.IsSuccessStatusCode) break;
                var content = await response.Content.ReadAsStringAsync();

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='background']/div[@id='contentPage']/div[@class='contentBox']/div/div/div/section/div/div/div/div/section/header/a");
                resultsList.AddRange(nodes.Select(node =>
                    $"{node.GetAttributeValue("href", "incorrect htmlNode query")}"));
            }

            return resultsList;
        }
    }
}
