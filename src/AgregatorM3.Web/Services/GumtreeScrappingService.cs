using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace AgregatorM3.Web.Services
{
    public class GumtreeScrappingService : IScrappingService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<List<string>> GetData(int priceMin, int priceMax)
        {
            var locationList = new List<string>{
                "wejnerta", "goszczyńskiego", "malczewskiego","pilicka", "widok+na+miasto", "panorama+miasta", "panorama+warszawy",
                //"lenartowicza","naruszewicza", "krasickiego", "ursynowska", "broniwoja",
                //"woronicza", "tyniecka", "szarotki", "konduktorkska", "joliot+curie","gandhiego",
                //"bytnara", "bukietowa", "modzelewskiego", "kolberga", "piaseczyńska", "stacja+metra","pole+mokotowskie","marzanny",
                //"boryszewska", "belgijska", "dworkowa", "morskie+oko", "rozana", "madalinskiego", "ludwika-narbutta",
                //"wisniowa", "rejtana", "bruna", "sandomierska", "łowicka", "kielecka", "opoczyńska", "asfaltowa", "falata",
                //"akacjowa", "balladyny", "dabrowskiego", "wiktorska", "gimnastyczna", "bełska", "garażowa",
                //"samochodowa", "domaniewska", "lutocińska", "ksawerów", "abramowskiego", "żywnego",
                //"bielawska", "wielicka", "kazimierzowska", "odolańska", "lewicka", "kwiatowa", "wrotkowa", "racławicka",
                //"szczekocińska", "wybieg", "piłkarska", "słoneczna", "olszewska", "chocimska", "bałuckiego"
            };

            var resultsList = new List<string>();

            for (int i = 0; i < locationList.Count; i++)
            {
                string gumtreeUrl = String.Concat("https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/mokotow/v1c9073l3200012p1?",
                    "q=", locationList[i], "&pr=", priceMin, ",", priceMax); // for private only add "&df=ownr&nr=3"

                var response = await client.GetAsync(gumtreeUrl);
                if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Niestety, nie znaleźliśmy żadnych wyników. Szukając ogłoszeń skorzystaj z poniższych sugestii.")) continue;
                if (content.Contains("Przepraszamy, ale ta strona nie istnieje")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//div[@class='viewport']/div[@class='containment']/div/div[@class='content']/section/div/div[@class='results list-view']/div[@class='view']/div/div").Descendants("a");

                foreach (var node in nodes)
                {
                    var linkResult = $"https://www.gumtree.pl{node.GetAttributeValue("href", "incorrect htmlNode query")}";
                    if (!resultsList.Contains(linkResult))
                    {
                        resultsList.Add(linkResult);
                    }
                }
            }
            return resultsList;
        }
    }
}
