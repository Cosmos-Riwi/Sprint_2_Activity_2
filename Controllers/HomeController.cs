using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Models;
using System.Diagnostics;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Home controller for the restaurant management system
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main dashboard page
        /// </summary>
        public IActionResult Index()
        {
            ViewData["Title"] = "Sistema de Restaurante - Dashboard";
            return View();
        }

        /// <summary>
        /// About page
        /// </summary>
        public IActionResult About()
        {
            ViewData["Title"] = "Acerca de - Sistema de Restaurante";
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
