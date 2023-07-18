using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using System.Diagnostics;

namespace Movie_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = new
            {
                name = "utsab",
                age = "23"

            };
            return View(data);
        }

        public IActionResult Fetch(string name, string age)
        {
            var data = new
            {
                name = name,
                age = age

            };
            return Json(data);
        }
        [Authorize(Roles = "User")]
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