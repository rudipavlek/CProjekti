using Microsoft.AspNetCore.Mvc;
using aspprvi.Models;
using System.Linq;

namespace aspprvi.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogPostDBContext _context;

        public BlogController(BlogPostDBContext context)
        {
            _context = context;
        }

        // Prikaz svih blog postova s opcionalnim pretraživanjem po naslovu
        public IActionResult Index(string searchTitle)
        {
            var posts = string.IsNullOrEmpty(searchTitle)
                ? _context.BlogPosts.ToList()
                : _context.BlogPosts.Where(p => p.title.Contains(searchTitle)).ToList();

            ViewData["SearchTitle"] = searchTitle;  // Dodajemo SearchTitle u ViewData

            return View(posts);
        }

        // Prikaz pojedinačnog blog posta
        public IActionResult Details(int id)
        {
            var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        // Prikaz forme za kreiranje novog blog posta
        public IActionResult Create()
        {
            var newPost = new Blog_Post();  // Inicijalizirajte model ako je potrebno
            return View(newPost);  // Proslijedite model u View
        }

        [HttpPost]
        public IActionResult Create(Blog_Post newPost)
        {
            if (ModelState.IsValid)
            {
                newPost.created_at = DateTime.Now;
                newPost.updated_at = DateTime.Now;

                _context.BlogPosts.Add(newPost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newPost);
        }

        // Prikaz forme za uređivanje postojećeg blog posta
        public IActionResult Edit(int id)
        {
            var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(int id, Blog_Post updatedPost)
        {
            if (id != updatedPost.id) return NotFound();

            if (ModelState.IsValid)
            {
                var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
                if (post == null) return NotFound();

                post.title = updatedPost.title;
                post.content = updatedPost.content;
                post.updated_at = DateTime.Now;

                _context.BlogPosts.Update(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(updatedPost);
        }

        // Brisanje blog posta
        public IActionResult Delete(int id)
        {
            var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
            if (post == null) return NotFound();

            _context.BlogPosts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
