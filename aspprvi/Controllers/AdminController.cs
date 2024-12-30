using aspprvi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspprvi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserDBContext _context;

        public AdminController(UserDBContext context)
        {
            _context = context;
        }

        

        // Prikaz svih korisnika
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
            
        }

        // Kreiranje novog korisnika
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newUser);
        }

        // Uređivanje postojećeg korisnika
        public IActionResult Edit(int id)
        {
            // Dohvati korisnika prema ID-u
            var user = _context.Users.SingleOrDefault(u => u.id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.id) return NotFound();

            // Provjerava da li je model ispravan
            if (ModelState.IsValid)
            {
                try
                {
                    // Dohvati korisnika iz baze prema ID-u
                    var userToUpdate = _context.Users.SingleOrDefault(u => u.id == id);

                    if (userToUpdate == null) return NotFound();

                    userToUpdate.username = updatedUser.username;
                    userToUpdate.email = updatedUser.email;
                    userToUpdate.password = updatedUser.password;
                    

                    // Praćenje objekta za promjene (Attach metodom)
                    _context.Users.Attach(userToUpdate);

                    // Označavamo da su podaci promijenjeni
                    _context.Entry(userToUpdate).State = EntityState.Modified;

                    // Spremanje promjena u bazu
                    _context.SaveChanges();

                    // Preusmjeravanje na listu korisnika
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Došlo je do pogreške pri ažuriranju korisnika.");
                }
            }

            // Ako model nije validan, vraćamo formu s greškom
            return View(updatedUser);
        }




        // Brisanje korisnika
        public IActionResult Delete(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.id == id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
