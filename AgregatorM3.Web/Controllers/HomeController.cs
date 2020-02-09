using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using HtmlAgilityPack;
using AgregatorM3.Web.Services;

namespace AgregatorM3.Web.Controllers
{
    public class HomeController : Controller
    {
       // private static HttpClient client = new HttpClient();
       // private static List<string> seenAdverts = ReadSeenData();
        private readonly IScrappingService _domImportaService;

        public HomeController(IScrappingService scrappingService)
        {
            _domImportaService = scrappingService;
        }

        public async Task<IActionResult> Index()
        {
            // USE ASYNC STREAMS

            var priceMin = 500000;
            var priceMax = 950000;

            var result = await _domImportaService.GetAddresses(priceMin, priceMax);
               // Gumtree(priceMin, priceMax, seenAdverts);

            return View(result);
        }

        private static List<string> ReadSeenData()
        {
            string textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.ConsoleApp\seen.txt";
            string[] lines = System.IO.File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

       

        //public static async Task Gumtree(int priceMin, int priceMax, List<string> blackListed)
        //{
        //    var locationList = new List<string>{
        //        "wejnerta", "goszczyńskiego", "malczewskiego","pilicka", "widok+na+miasto", "panorama+miasta", "panorama+warszawy",
        //        "lenartowicza","naruszewicza", "krasickiego", "ursynowska", "broniwoja",
        //        "woronicza", "tyniecka", "szarotki", "konduktorkska", "joliot+curie","gandhiego",
        //        "bytnara", "bukietowa", "modzelewskiego", "kolberga", "piaseczyńska", "stacja+metra","pole+mokotowskie","marzanny",
        //        "boryszewska", "belgijska", "dworkowa", "morskie+oko", "rozana", "madalinskiego", "ludwika-narbutta",
        //        "wisniowa", "rejtana", "bruna", "sandomierska", "łowicka", "kielecka", "opoczyńska", "asfaltowa", "falata",
        //        "akacjowa", "balladyny", "dabrowskiego", "wiktorska", "gimnastyczna", "bełska", "garażowa",
        //        "samochodowa", "domaniewska", "lutocińska", "ksawerów", "abramowskiego", "żywnego",
        //        "bielawska", "wielicka", "kazimierzowska", "odolańska", "lewicka", "kwiatowa", "wrotkowa", "racławicka",
        //        "szczekocińska", "wybieg", "piłkarska", "słoneczna", "olszewska", "chocimska", "bałuckiego"
        //    };

        //    var resultsList = new List<string>();

        //    for (int i = 0; i < locationList.Count; i++)
        //    {
        //        string gumtreeUrl = String.Concat("https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/mokotow/v1c9073l3200012p1?",
        //            "q=", locationList[i], "&pr=", priceMin, ",", priceMax, "&df=ownr&nr=3");

        //        var response = await client.GetAsync(gumtreeUrl);
        //        if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
        //        var content = await response.Content.ReadAsStringAsync();

        //        if (content.Contains("Niestety, nie znaleźliśmy żadnych wyników. Szukając ogłoszeń skorzystaj z poniższych sugestii.")) continue;
        //        if (content.Contains("Przepraszamy, ale ta strona nie istnieje")) continue;

        //        var htmDocument = new HtmlDocument();
        //        htmDocument.LoadHtml(content);
        //        var nodes = htmDocument.DocumentNode.
        //            SelectNodes("//div[@class='viewport']/div[@class='containment']/div/div[@class='content']/section/div/div[@class='results list-view']/div[@class='view']/div/div").Descendants("a");

        //        foreach (var node in nodes)
        //        {
        //            var linkResult = $"https://www.gumtree.pl{node.GetAttributeValue("href", "incorrect htmlNode query")}";
        //            if (!resultsList.Contains(linkResult) && !blackListed.Contains(linkResult))
        //            {
        //                resultsList.Add(linkResult);
        //                Console.WriteLine($"{locationList[i]}: {linkResult}");
        //            }
        //        }
        //    }


        //}
    }
}
