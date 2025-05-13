using Microsoft.AspNetCore.Mvc;
using ST10404431CLDV6211POE.Models;
using System.Diagnostics;

namespace ST10404431CLDV6211POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventEaseDBContext _context;

        public HomeController(ILogger<HomeController> logger, EventEaseDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var events = _context.Events
                .OrderBy(e => e.EventDate)
                .Take(3)
                .ToList();

            var venues = _context.Venues
                .OrderByDescending(v => v.Capacity)
                .Take(3)
                .ToList();

            var model = Tuple.Create(events, venues);
            return View(model);
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
