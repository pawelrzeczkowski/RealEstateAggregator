using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Exceptionless;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class GumtreeScrappingService : IScrappingService
    {
        private readonly HttpClient _httpClient;

        public GumtreeScrappingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            var locationList = new List<string>{
                "wejnerta", "goszczyńskiego", "malczewskiego","pilicka", "widok+na+miasto", "panorama+miasta", "panorama+warszawy",
                "lenartowicza","naruszewicza", "krasickiego", "ursynowska", "broniwoja",
                "woronicza", "tyniecka", "szarotki", "konduktorkska", "joliot+curie","gandhiego",
                "bytnara", "bukietowa", "modzelewskiego", "kolberga", "piaseczyńska", "stacja+metra","pole+mokotowskie","marzanny",
                "boryszewska", "belgijska", "dworkowa", "morskie+oko", "rozana", "madalinskiego", "ludwika-narbutta",
                "wisniowa", "rejtana", "bruna", "sandomierska", "łowicka", "kielecka", "opoczyńska", "asfaltowa", "falata",
                "akacjowa", "balladyny", "dabrowskiego", "wiktorska", "gimnastyczna", "bełska", "garażowa",
                "samochodowa", "domaniewska", "lutocińska", "ksawerów", "abramowskiego", "żywnego",
                "bielawska", "wielicka", "kazimierzowska", "odolańska", "lewicka", "kwiatowa", "wrotkowa", "racławicka",
                "szczekocińska", "wybieg", "piłkarska", "słoneczna", "olszewska", "chocimska", "bałuckiego"
            };

            foreach (var location in locationList)
            {
                var referer = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/mokotow/v1c9073l3200012p1";
                var gumtreeUrl = $"{referer}?q={location}&pr={searchModel.PriceFrom},{searchModel.PriceTo}"; // for private only add "&df=ownr&nr=3"

                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,pl;q=0.8");
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                Thread.Sleep(300);

                var response = await _httpClient.GetAsync(gumtreeUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ExceptionlessClient.Default.SubmitLog("response status code not 200: " + response.Content.ReadAsStringAsync());
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Niestety, nie znaleźliśmy żadnych wyników. Szukając ogłoszeń skorzystaj z poniższych sugestii.")) continue;
                if (content.Contains("Przepraszamy, ale ta strona nie istnieje")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//div[@class='viewport']/div[@class='containment']/div/div[@class='content']/section/div/div[@class='results list-view']/div[@class='view']/div/div").Descendants("a");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Gumtree, htmlContent: " + content);
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("Gumtree", 
                        $"https://www.gumtree.pl{node.GetAttributeValue("href", "incorrect htmlNode query: Gumtree")}");
                }
            }
        }
    }
}
