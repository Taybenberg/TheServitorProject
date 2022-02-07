using DestinyInfocardsDatabase.ORM.LostSectors;
using Microsoft.EntityFrameworkCore;

namespace DestinyInfocardsDatabase
{
    public class InfocardsContext : DbContext
    {
        public DbSet<LostSectorsDailyReset> LostSectorsDailyResets { get; set; }
        public DbSet<LostSector> LostSectors { get; set; }

        public InfocardsContext(DbContextOptions<InfocardsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<long>();
        }
    }
}