using ContactApp.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TelemetryClient _telemetry;
        private readonly ILogger<HomeController> _logger;

        public HomeController(TelemetryClient telemetry, ILogger<HomeController> logger)
        {
            _telemetry = telemetry;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Index page logged");
            _telemetry.TrackTrace("User visited Home page");
            _telemetry.TrackEvent("HomePageLoaded");
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
