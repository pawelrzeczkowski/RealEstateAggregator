using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace AgregatorM3.Web.Services
{
    public class OtoDomScrappingService : IScrappingService
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
                    $"https://www.otodom.pl/sprzedaz/mieszkanie/?search%5Bfilter_float_price%3Afrom%5D={priceMin}&search%5Bfilter_float_price%3Ato%5D={priceMax}"
                    + $"&search%5Bfilter_float_price_per_m%3Ato%5D=13500&search%5Bfilter_float_m%3Afrom%5D=60&search%5Bfilter_float_m%3Ato%5D=100&search%5B"
                    + $"filter_enum_rooms_num%5D%5B0%5D=3&search%5Bfilter_enum_rooms_num%5D%5B1%5D=4&search%5Bfilter_enum_market%5D%5B0%5D=secondary&search%5B"
                    + $"filter_enum_floor_no%5D%5B0%5D=floor_3&search%5Bfilter_enum_floor_no%5D%5B1%5D=floor_4&search%5Bfilter_enum_floor_no%5D%5B2%5D=floor_5"
                    + $"&search%5Bfilter_enum_floor_no%5D%5B3%5D=floor_6&search%5Bfilter_enum_floor_no%5D%5B4%5D=floor_7&search%5Bfilter_enum_floor_no%5D%5B5%5D"
                    + $"=floor_8&search%5Bfilter_enum_floor_no%5D%5B6%5D=floor_9&search%5Bfilter_enum_floor_no%5D%5B7%5D=floor_10&search%5Bfilter_enum_floor_no%5D%5B8%5D"
                    + $"=floor_higher_10&search%5Bdescription%5D=1&locations%5B0%5D%5Bregion_id%5D=7&locations%5B0%5D%5Bsubregion_id%5D=197&locations%5B0%5D%5Bcity_id%5D"
                    + $"=26&locations%5B0%5D%5Bdistrict_id%5D=39&locations%5B1%5D%5Bregion_id%5D=7&locations%5B1%5D%5Bsubregion_id%5D=197&locations%5B1%5D%5Bcity_id%5D=26"
                    + $"&locations%5B1%5D%5Bdistrict_id%5D=7365&nrAdsPerPage=72" + paging;

                client.DefaultRequestHeaders.Add("Accept", "text/html");
                var response = await client.GetAsync(url);
                if (response.RequestMessage.RequestUri.ToString().Length < url.Length - 20) break;
                if (!response.IsSuccessStatusCode) break;
                var content = await response.Content.ReadAsStringAsync();

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.SelectNodes(
                    "//div[@id='listContainer']/main/section[@id='body-container']/div/div/div/div/article/div[@class='offer-item-details']/header/h3/a");
                resultsList.AddRange(nodes.Select(node =>
                    $"{node.GetAttributeValue("href", "incorrect htmlNode query")}"));
            }

            return resultsList;
        }
    }
}
