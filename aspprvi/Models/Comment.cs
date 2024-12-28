using Microsoft.EntityFrameworkCore;

namespace aspprvi.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int user_id { get; set; }
        public int blogpost_id { get; set; }
    }

    public class CommentDBContext : DbContext
    {
        public CommentDBContext(DbContextOptions<CommentDBContext> options)
            : base(options)
        { }

        public DbSet<Comment> Comments { get; set; }
    }
}
