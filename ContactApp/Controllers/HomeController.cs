using ContactApp.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TelemetryClient _telemetry;

        public HomeController(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }
        public IActionResult Index()
        {
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
