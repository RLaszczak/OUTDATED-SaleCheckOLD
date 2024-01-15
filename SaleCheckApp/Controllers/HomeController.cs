using Microsoft.AspNetCore.Mvc;
using SaleCheckApp.Models;
using SaleCheckApp.Services;
using System.Diagnostics;
using HtmlAgilityPack;

namespace SaleCheckApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISaleCheckDataService _SaleCheckDataService;

        public HomeController(ILogger<HomeController> logger, ISaleCheckDataService SaleCheckDataService)
        {
            _logger = logger;
            _SaleCheckDataService = SaleCheckDataService;
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}