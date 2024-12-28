using Microsoft.EntityFrameworkCore;

namespace aspprvi.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}