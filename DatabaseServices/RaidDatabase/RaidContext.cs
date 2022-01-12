using Microsoft.EntityFrameworkCore;
using RaidDatabase.ORM;

namespace RaidDatabase
{
    public class RaidContext : DbContext
    {
        public DbSet<Raid> Raids { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public RaidContext(DbContextOptions<RaidContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<long>();
        }
    }
}