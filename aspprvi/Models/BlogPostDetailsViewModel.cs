using Microsoft.AspNetCore.Mvc;

namespace aspprvi.Models
{
    public class BlogPostDetailsViewModel
    {
        public Blog_Post BlogPost { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
