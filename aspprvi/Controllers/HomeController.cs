using aspprvi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace aspprvi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
                private readonly UserDBContext _context;

        public HomeController(ILogger<HomeController> logger, UserDBContext context)
        {
            _logger = logger;
            _context = context;

        }
        public IActionResult Index()
        {
            // Dohvati korisnički ID iz sesije
            var userId = HttpContext.Session.GetString("UserId");

            // Ako je korisnik prijavljen, prikaži njegove podatke
            if (!string.IsNullOrEmpty(userId))
            {
                ViewBag.UserId = userId; // Možeš koristiti ovu vrijednost za prikaz na stranici
                ViewBag.User = _context.Users.SingleOrDefault(u => u.id == int.Parse(userId));
                ViewBag.username = ViewBag.User.username;
            }

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

        public IActionResult TestConnection()
        {
            try
            {
                // Pokušavamo izvršiti jednostavan upit koji provjerava vezu
                _context.Database.OpenConnection();
                _context.Database.CloseConnection();

                return Content("Uspješno ste povezani na bazu.");
            }
            catch (Exception ex)
            {
                return Content($"Greška u povezivanju: {ex.Message}");
            }
        }
    }
}
