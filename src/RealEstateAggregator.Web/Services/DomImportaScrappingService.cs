using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
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
                var referer = "https://www.domiporta.pl/mieszkanie/sprzedam/mazowieckie/warszawa/";
                var domImportaUrl = $"{referer}{location}?" + 
                                    $"Surface.From={searchModel.SurfaceFrom}&Surface.To={searchModel.SurfaceTo}&Price.From={searchModel.PriceFrom}&Price.To={searchModel.PriceTo}" + 
                                    $"&Rooms.From={searchModel.RoomsFrom}&Rooms.To={searchModel.RoomsTo}&PricePerMeter.To={searchModel.PricePerMeterTo}";

                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,pl;q=0.8");
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                Thread.Sleep(300);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await _httpClient.GetAsync(domImportaUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ExceptionlessClient.Default.SubmitLog(
                        $"response status code not 200: at: {location}, content: {response.Content.ReadAsStringAsync()}");
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Znaleziono 0 ogłoszeń")) continue;
                if (content.Contains("Brak wyników spełniających Twoje kryteria wyszukiwania")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//main/div/div/div/div/div/div[@class='listing__container']/div/ul/li/article[@class='sneakpeak']");

                if (nodes == null)
                {
                    ExceptionlessClient.Default.SubmitLog("incorrect html select node query: Domimporta, htmlContent: " + content);
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
