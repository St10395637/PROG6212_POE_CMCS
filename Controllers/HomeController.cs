// Importing necessary namespaces
using Claiming_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Claiming_System.Controllers
{
    // HomeController handles the main actions for the home page and error page of the application
    public class HomeController : Controller
    {
        // Private readonly field to store the logger for logging purposes
        private readonly ILogger<HomeController> _logger;

        // Constructor that injects the logger service into the controller
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // The Index action returns the main view of the home page
        public IActionResult Index()
        {
            return View();  // Returns the Index view (the default view for this action)
        }

        // The Privacy action returns a view that displays privacy-related information
        public IActionResult Privacy()
        {
            return View();  // Returns the Privacy view
        }

        // The Error action is used to handle errors, with a custom error page
        // The ResponseCache attribute disables caching for this error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Returns an error view with the current request identifier (useful for debugging)
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
