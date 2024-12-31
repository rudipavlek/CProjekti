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
        private readonly BlogPostDBContext _contextBlogPost;
        private readonly CommentDBContext _contextComment;

        public AdminController(UserDBContext context, BlogPostDBContext contextBlogPost, CommentDBContext contextComment)
        {
            _context = context;
            _contextBlogPost = contextBlogPost;
            _contextComment = contextComment;
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
            //Pronadi korisnika
            var user = _context.Users.SingleOrDefault(u => u.id == id);
            if (user == null) return NotFound();


            ////Pronadi komentare
            //var comments = _contextComment.Comments.Where(c => c.user_id == id).ToList();
            //Console.WriteLine($"Found {comments.Count} comments for user with id {id}.");

            //foreach (var comment in comments)
            //{
            //    Console.WriteLine($"Comment ID: {comment.id}, Content: {comment.content}");

            //    comment.user_id = 0;
            //    _contextComment.Comments.Update(comment); // Ažuriraj zapis u bazi
            //}

            ////Pronadi blogpostove korisnika
            //var blogPosts = _contextBlogPost.BlogPosts.Where(p => p.user_id==id).ToList();
            //Console.WriteLine($"Found {blogPosts.Count} blog posts for user with id {id}.");

            //foreach (var post in blogPosts)
            //{
            //    Console.WriteLine($"Blog Post ID: {post.id}, Title: {post.title}");

            //    post.user_id = 0;
            //    _contextBlogPost.BlogPosts.Update(post); // Ažuriraj zapis u bazi
            //}

            

            //// Ažuriraj promjene u bazi
            //try
            //{
            //    _contextComment.SaveChanges();
            //    _contextBlogPost.SaveChanges();
                
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error while saving changes: {ex.Message}");
            //    return StatusCode(500, "Internal server error while updating blog posts or comments.");
            //}

            // Obriši korisnika
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                Console.WriteLine($"User with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting user: {ex.Message}");
                return StatusCode(500, "Internal server error while deleting user.");
            }
            return RedirectToAction("Index");
        }
    }
}
