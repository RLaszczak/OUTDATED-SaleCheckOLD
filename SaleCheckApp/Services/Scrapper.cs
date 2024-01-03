using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PogodaApp.Models;
using PogodaApp.Services;

public class Scrapper
{
    private readonly IWeatherDataService _weatherDataService;
    private readonly IWebDriver _webDriver;
    public string WebsiteString { get; set; }

    public Scrapper(IWeatherDataService weatherDataService)
    {
        _weatherDataService = weatherDataService;
        var driver = new EdgeDriver();
        driver.Url = "https://allegro.pl/kategoria/modelarstwo-1061/";

        WebsiteString = driver.PageSource;
    }

    public async Task Scrap()
    {
        var document = new HtmlDocument();
        document.LoadHtml(WebsiteString);

        // Zaktualizowane selektory do pobierania nazwy, ceny i linku
        var productContainers = document.DocumentNode.SelectNodes("//div[contains(@class,'mg9e_16') and contains(@class,'mj7a_16') and contains(@class,'mg9e_24_l') and contains(@class,'mj7a_24_l')]");

        if (productContainers != null)
        {
            List<ScrapedDataModel> scrapedDataList = new List<ScrapedDataModel>();

            foreach (var container in productContainers)
            {
                ScrapedDataModel data = new ScrapedDataModel();

                // Pobieranie nazwy, ceny i linku
                var productNameNode = container.SelectSingleNode(".//span[contains(@class, 'product-name')]");
                var priceNode = container.SelectSingleNode(".//span[contains(@class, 'price')]");
                var linkNode = container.SelectSingleNode(".//a[contains(@class, 'product-link')]");

                // Sprawdzenie, czy węzły istnieją przed próbą pobrania ich wartości
                if (productNameNode != null)
                {
                    data.AllegroName = productNameNode.InnerText.Trim();
                }

                if (priceNode != null)
                {
                    data.Price = priceNode.InnerText.Trim();
                }

                if (linkNode != null)
                {
                    data.Link = linkNode.GetAttributeValue("href", "").Trim();
                }

                scrapedDataList.Add(data);
            }

            foreach (var data in scrapedDataList)
            {
                await _weatherDataService.CreateAsync(data);
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono produktów.");
        }

        _webDriver.Close();
    }

    private static async Task<string> CallUrl(string fullUrl)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(fullUrl);
        return response;
    }
}
