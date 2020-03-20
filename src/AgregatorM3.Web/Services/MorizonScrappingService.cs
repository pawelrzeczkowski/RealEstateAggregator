using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AgregatorM3.Web.Models;
using HtmlAgilityPack;

namespace AgregatorM3.Web.Services
{
    public class MorizonScrappingService : IScrappingService
    {
        private readonly HttpClient client = new HttpClient();

        public async IAsyncEnumerable<string> GetData(SearchModel searchModel)
        {
            for (var page = 1; page < 7; page++)
            {
                var paging = string.Empty;
                if (page > 1) paging = $"&page={page}";
                var url =
                    $"https://www.morizon.pl/mieszkania/warszawa/mokotow/?ps%5Bprice_from%5D={searchModel.PriceFrom}&ps%5Bprice_to%5D={searchModel.PriceTo}&ps%5Bprice_m2_to%5D={searchModel.PricePerMeterTo}"
                    + $"&ps%5Bliving_area_from%5D={searchModel.SurfaceFrom}&ps%5Bliving_area_to%5D={searchModel.SurfaceTo}&ps%5Bnumber_of_rooms_from%5D=" 
                    + $"{searchModel.RoomsFrom}&ps%5Bnumber_of_rooms_to%5D={searchModel.RoomsTo}&ps%5Bhas_parking_places%5D=1&ps%5Bmarket_type%5D%5B0%5D=2" + paging;

                client.DefaultRequestHeaders.Add("Accept", "text/html");
                var response = await client.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length != url.Length) break;
                if (!response.IsSuccessStatusCode) break;
                var content = await response.Content.ReadAsStringAsync();

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='background']/div[@id='contentPage']/div[@class='contentBox']/div/div/div/section/div/div/div/div/section/header/a");
                foreach (var node in nodes)
                {
                    yield return $"{node.GetAttributeValue("href", "incorrect htmlNode query")}";
                }
            }
        }
    }
}
