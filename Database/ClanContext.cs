using Database.ORM;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class ClanContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityUserStats> ActivityUserStats { get; set; }

        public ClanContext(DbContextOptions<ClanContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
