using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net;

namespace AgregatorM3.ConsoleApp
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private static List<string> seenAdverts = ReadSeenData();

        static void Main(string[] args)
        {
            var priceMin = 700000;
            var priceMax = 950000;

            Komornik();

            Task.WaitAll(
                Domimporta(priceMin, priceMax),
                Gumtree(priceMin, priceMax)
                );

            Console.WriteLine("--------------");
            Console.WriteLine("KONIEC");
            Console.ReadKey();
        }

        static List<string> ReadSeenData()
        {
            string textFile = @"C:\Code\AgregatorM3\src\AgregatorM3.ConsoleApp\seen.txt";
            string[] lines = File.ReadAllLines(textFile);

            return lines.OfType<string>().ToList();
        }

        public static async Task Domimporta(int priceMin, int priceMax)
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

            var resultsList = new List<string>();

            for (int i = 0; i < locationList.Count; i++)
            {
                string domImportaUrl = String.Concat("https://www.domiporta.pl/mieszkanie/sprzedam/mazowieckie/warszawa/", locationList[i],
                    "?Surface.From=55&Surface.To=110&Price.From=400000&Price.To=900000&Rooms.From=3&Rooms.To=4&PricePerMeter.To=13000");

                var response = await client.GetAsync(domImportaUrl);
                if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("Znaleziono 0 ogłoszeń")) continue;
                if (content.Contains("Brak wyników spełniających Twoje kryteria wyszukiwania")) continue;

                var htmDocument = new HtmlDocument();
                htmDocument.LoadHtml(content);
                var nodes = htmDocument.DocumentNode.
                    SelectNodes("//main/div/div/div/div/div/div[@class='listing__container']/div/ul/li/article");
                foreach (var node in nodes)
                {
                    var linkResult = $"https://www.domiporta.pl{node.GetAttributeValue("data-href", "incorrect htmlNode query")}";
                    if (!resultsList.Contains(linkResult) && !seenAdverts.Contains(linkResult))
                    {
                        resultsList.Add(linkResult);
                        Console.WriteLine($"{locationList[i]}: {linkResult}");
                    }
                }
            }
        }

        public static async Task Gumtree(int priceMin, int priceMax)
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

            var resultsList = new List<string>();

            for (int i = 0; i < locationList.Count; i++)
            {
                string gumtreeUrl = String.Concat("https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/mokotow/v1c9073l3200012p1?",
                    "q=", locationList[i], "&pr=", priceMin, ",", priceMax, "&df=ownr&nr=3");

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
                    if (!resultsList.Contains(linkResult) && !seenAdverts.Contains(linkResult))
                    {
                        resultsList.Add(linkResult);
                        Console.WriteLine($"{locationList[i]}: {linkResult}");
                    }
                }
            }


        }

        public static async Task Komornik()
        {
            string baseUrl = "https://licytacje.komornik.pl/Notice/Search";
            var response = await client.GetAsync(baseUrl);
            if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
            var requestContent = new FormUrlEncodedContent(new[]
            {
                    //new KeyValuePair<string, string>("__RequestVerificationToken", "HnAl4YQZMMNjnD9ugDsLZgZ2KjrPyweavipiDyb2UdqfOJ_ZnHlJMfh8oiFugHLkgdWd0FUGL2T5icUE1slwEDlclbro-983XXtqmjddRz41"),
                    new KeyValuePair<string, string>("City", "warszawa"),
                    new KeyValuePair<string, string>("PriceFrom", "300000,00"),
                    new KeyValuePair<string, string>("PriceTo", "700000,00"),
                    new KeyValuePair<string, string>("PublicationDateFrom", "10.01.2020"),
                    new KeyValuePair<string, string>("CategoryId", ""),
                    new KeyValuePair<string, string>("MobilityCategoryId", ""),
                    new KeyValuePair<string, string>("PropertyCategoryId", ""),
                    new KeyValuePair<string, string>("tbx-province", ""),
                    new KeyValuePair<string, string>("ProvinceId", ""),
                    new KeyValuePair<string, string>("AuctionsDate", ""),
                    new KeyValuePair<string, string>("Words", ""),
                    new KeyValuePair<string, string>("ItemMin", ""),
                    new KeyValuePair<string, string>("ItemMax", ""),
                    new KeyValuePair<string, string>("ElkrkSelected", ""),
                    new KeyValuePair<string, string>("ElkrkStatus", ""),
                    new KeyValuePair<string, string>("OfficeId", ""),
                    new KeyValuePair<string, string>("JudgmentId", ""),
                    new KeyValuePair<string, string>("PublicationDateTo", ""),
                    new KeyValuePair<string, string>("StartDateFrom", ""),
                    new KeyValuePair<string, string>("StartDateTo", ""),
                    new KeyValuePair<string, string>("SumMin", ""),
                    new KeyValuePair<string, string>("SumMax", ""),
                    new KeyValuePair<string, string>("Vat", ""),
                    new KeyValuePair<string, string>("TypeOfAuction", "")
                });
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) })
            {
                cookieContainer.Add(new Uri(baseUrl), new Cookie("CookieName", "cookie_value"));
                response = await client.PostAsync(baseUrl, requestContent);
            }

            if (!response.IsSuccessStatusCode) Console.WriteLine("incorrect URL, skipping...");
            var content = await response.Content.ReadAsStringAsync();


            if (content.Contains("Niestety, nie znaleźliśmy żadnych wyników. Szukając ogłoszeń skorzystaj z poniższych sugestii.")) return;
            if (content.Contains("Przepraszamy, ale ta strona nie istnieje")) return;

            var htmDocument = new HtmlDocument();
            htmDocument.LoadHtml(content);
            var nodes = htmDocument.DocumentNode.
                SelectNodes("//div[@class='viewport']/div[@class='containment']/div/div[@class='content']/section/div/div[@class='results list-view']/div[@class='view']/div/div").Descendants("a");

            var resultsList = new List<string>();

            foreach (var node in nodes)
            {
                var linkResult = $"https://www.gumtree.pl{node.GetAttributeValue("href", "incorrect htmlNode query")}";
                if (!resultsList.Contains(linkResult) && !seenAdverts.Contains(linkResult))
                {
                    resultsList.Add(linkResult);
                    Console.WriteLine(linkResult);
                }
            }
        }
    }
 }