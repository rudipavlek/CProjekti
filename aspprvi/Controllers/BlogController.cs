using Microsoft.AspNetCore.Mvc;
using aspprvi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace aspprvi.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly BlogPostDBContext _context;
        private readonly CommentDBContext _contextComment;

        public BlogController(BlogPostDBContext context, CommentDBContext contextComment)
        {
            _context = context;
            _contextComment = contextComment;
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

            var comments = _contextComment.Comments
                        .Where(c => c.blogpost_id == id)
                        .ToList();
            var viewModel = new BlogPostDetailsViewModel
            {
                BlogPost = post,
                Comments = comments
            };

            return View(viewModel);
        }

        // Odgovor pojedinačnog blog posta
        public IActionResult Reply(int id)
        {
            var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        public IActionResult Reply(int blogpost_id, Comment newComment)
        {
            if (ModelState.IsValid)
            {
                //var post = _context.BlogPosts.SingleOrDefault(p => p.id == id);
                //if (post == null) return NotFound();

                var userId = HttpContext.Session.GetString("UserId");
                if (!string.IsNullOrEmpty(userId))
                {
                    newComment.user_id = int.Parse(userId);
                }
                ///newComment.id.Equals(null);
                newComment.blogpost_id = blogpost_id; // Povezivanje komentara sa blog postom
                newComment.created_at = DateTime.Now;
                newComment.updated_at = DateTime.Now;
                _contextComment.Comments.Add(newComment);
                _contextComment.SaveChanges();

                return RedirectToAction("Index", "Blog");
            }

            // Ako nešto nije validno, ponovo prikazujemo formu
            return View(newComment);
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
            var userId = HttpContext.Session.GetString("UserId");

            if (ModelState.IsValid)
            {
                newPost.created_at = DateTime.Now;
                newPost.updated_at = DateTime.Now;
                newPost.user_id = int.Parse(HttpContext.Session.GetString("UserId"));
                

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
