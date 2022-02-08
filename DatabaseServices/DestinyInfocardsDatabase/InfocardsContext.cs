using DestinyInfocardsDatabase.ORM.LostSectors;
using DestinyInfocardsDatabase.ORM.Resources;
using DestinyInfocardsDatabase.ORM.Xur;
using Microsoft.EntityFrameworkCore;

namespace DestinyInfocardsDatabase
{
    public class InfocardsContext : DbContext
    {
        public DbSet<LostSectorsDailyReset> LostSectorsDailyResets { get; set; }
        public DbSet<LostSector> LostSectors { get; set; }

        public DbSet<XurInventory> XurInventories { get; set; }
        public DbSet<XurItem> XurItems { get; set; }

        public DbSet<VendorsDailyReset> VendorsDailyResets { get; set; }
        public DbSet<ResourceItem> ResourceItems { get; set; }

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