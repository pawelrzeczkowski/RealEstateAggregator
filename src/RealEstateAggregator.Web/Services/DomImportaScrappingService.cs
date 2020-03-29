using System;
using System.Collections.Generic;
using System.Net.Http;
using Exceptionless;
using RealEstateAggregator.Web.Models;
using HtmlAgilityPack;

namespace RealEstateAggregator.Web.Services
{
    public class DomImportaScrappingService : IScrappingService
    {
        private readonly HttpClient _httpClient;

        public DomImportaScrappingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            var locationList = new List<string>{
                "aleksandra -wejnerta", "goszczynskiego-seweryna", "antoniego-malczewskiego","pilicka",
                "lenartowicza-teofila","adama-naruszewicza", "krasickiego-ignacego", "ursynowska", "broniwoja",
                "woronicza", "tyniecka", "szarotki", "konduktorkska", "fryderyka-joliot-curie","gandhiego-mahatmy",
                "janka-bytnara-rudego", "bukietowa", "zygmunta-modzelewskiego", "oskara-kolberga", "piaseczynska",
                "kolberga-oskara", "wierzbno-stacja-metra", "pole-mokotowskie-stacja-metra","okecka","marzanny",
                "boryszewska", "belgijska", "dworkowa", "morskie oko", "rozana", "madalinskiego", "ludwika-narbutta",
                "wisniowa", "rejtana", "bruna", "sandomierska", "lowicka", "kielecka", "opoczynska", "asfaltowa", "falata",
                "akacjowa", "balladyny", "jaroslawa-dabrowskiego", "wiktorska", "gimnastyczna", "belska", "garazowa",
                "samochodowa", "domaniewska", "lutocinska", "ksawerow", "edwarda-jozefa-abramowskiego", "wojciecha-zywnego",
                "bielawska", "wielicka", "kazimierzowska", "odolanska", "lewicka", "kwiatowa", "wrotkowa", "raclawicka",
                "szczekocinska", "wybieg", "pilkarska", "sloneczna", "olszewska", "chocimska", "michala-baluckiego"
            };

            foreach (var location in locationList)
            {
                var domImportaUrl = $"https://www.domiporta.pl/mieszkanie/sprzedam/mazowieckie/warszawa/{location}?" + 
                                    $"Surface.From={searchModel.SurfaceFrom}&Surface.To={searchModel.SurfaceTo}&Price.From={searchModel.PriceFrom}&Price.To={searchModel.PriceTo}" + 
                                    $"&Rooms.From={searchModel.RoomsFrom}&Rooms.To={searchModel.RoomsTo}&PricePerMeter.To={searchModel.PricePerMeterTo}";

                var response = await _httpClient.GetAsync(domImportaUrl);
                if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Znaleziono 0 ogłoszeń")) continue;
                if (content.Contains("Brak wyników spełniających Twoje kryteria wyszukiwania")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//main/div/div/div/div/div/div[@class='listing__container']/div/ul/li/article");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Domimporta");
                }

                foreach (var node in nodes)
                {
                    yield return new ResultModel("DomImporta", 
                        $"https://www.domiporta.pl{node.GetAttributeValue("data-href", "incorrect htmlNode query: DomImporta")}");                
                }
            }
        }
    }
}
