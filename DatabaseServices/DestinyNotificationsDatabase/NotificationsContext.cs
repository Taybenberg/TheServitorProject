using Microsoft.EntityFrameworkCore;

namespace DestinyNotificationsDatabase
{
    public class NotificationsContext : DbContext
    {
        public NotificationsContext(DbContextOptions<NotificationsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<long>();
        }
    }
}