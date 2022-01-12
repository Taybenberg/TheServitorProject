using ClanActivitiesDatabase.ORM;
using Microsoft.EntityFrameworkCore;

namespace ClanActivitiesDatabase
{
    public class ClanActivitiesContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityUserStats> ActivityUserStats { get; set; }

        public ClanActivitiesContext(DbContextOptions<ClanActivitiesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
