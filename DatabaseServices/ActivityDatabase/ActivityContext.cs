using ActivityDatabase.ORM;
using Microsoft.EntityFrameworkCore;

namespace ActivityDatabase
{
    public class ActivityContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<long>();
        }
    }
}