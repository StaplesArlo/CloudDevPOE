using System.Diagnostics;
using ST10404431CLDV6211POE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ST10404431CLDV6211POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        public IActionResult BookEvent() => View();
        public IActionResult Venues() => View();
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
