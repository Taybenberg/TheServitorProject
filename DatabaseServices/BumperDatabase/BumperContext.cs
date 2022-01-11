using Microsoft.EntityFrameworkCore;
using BumperDatabase.ORM;

namespace BumperDatabase
{
    public class BumperContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bump> Bumps { get; set; }

        public BumperContext(DbContextOptions<BumperContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<long>();
        }
    }
}