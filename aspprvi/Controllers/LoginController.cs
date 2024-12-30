using aspprvi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace aspprvi.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDBContext _context;

        public LoginController(UserDBContext context)
        {
            _context = context;
        }

        // Prikaz login forme
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                // Ako je korisnik pronađen, kreiraj claims za korisnika
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("UserId", user.id.ToString()) // Dodajemo korisnički ID u claims
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Ulogiraj korisnika
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Spremi podatke o korisniku u cookies ako je potrebno
                // Prikazat ćemo poruku dobrodošlice sa podacima o korisniku (ID)
                HttpContext.Session.SetString("UserId", user.id.ToString());

                return RedirectToAction("Index", "Home"); // Nakon prijave preusmjeri na početnu
            }

            ViewBag.Error = "Pogrešno korisničko ime ili lozinka";
            return View();
        }

        // Odjava korisnika
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }
    }
}
