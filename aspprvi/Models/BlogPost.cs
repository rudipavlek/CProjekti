using Microsoft.EntityFrameworkCore;

namespace aspprvi.Models
{
    public class Blog_Post
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int user_id { get; set; }
    }

    public class BlogPostDBContext : DbContext
    {
        public BlogPostDBContext(DbContextOptions<BlogPostDBContext> options)
            : base(options)
        { }

        public DbSet<Blog_Post> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapiranje klase na tablicu u bazi s ispravnim imenom
            modelBuilder.Entity<Blog_Post>()
                .ToTable("blog_posts"); // Ako tablica u bazi ima naziv 'blog_posts'
        }
    }
}
