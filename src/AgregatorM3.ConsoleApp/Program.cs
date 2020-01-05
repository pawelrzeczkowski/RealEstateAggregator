using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AgregatorM3.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var locationList = new List<string>{
                "aleksandra-wejnerta", "goszczynskiego-seweryna", "antoniego-malczewskiego","pilicka",
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

            Task.WaitAll(Domimporta(locationList));

            Console.WriteLine("--------------");
            Console.WriteLine("KONIEC");
            Console.ReadKey();
        }

        public static async Task Domimporta(List<string> locationList)
        {
            for (int i = 0; i < locationList.Count; i++)
            {
                string domImportaUrl = String.Concat("https://www.domiporta.pl/mieszkanie/sprzedam/mazowieckie/warszawa/", locationList[i], 
                    "?Surface.From=55&Surface.To=110&Price.From=400000&Price.To=900000&Rooms.From=3&Rooms.To=4&PricePerMeter.To=13000");

                var client = new HttpClient();
                var response = await client.GetAsync(domImportaUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Znaleziono 0 ogłoszeń")) continue;
                if (content.Contains("Brak wyników spełniających Twoje kryteria wyszukiwania")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//main/div/div/div/div/div/div[@class='listing__container']/div/ul/li/article");
                foreach (var node in nodes)
                {
                    Console.WriteLine($"{locationList[i]}: {node.GetAttributeValue("data-href", "incorrect htmlNode query")}");
                }
            }
        }
    }
}