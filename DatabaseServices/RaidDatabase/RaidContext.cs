using Microsoft.EntityFrameworkCore;

namespace RaidDatabase
{
    public class RaidContext : DbContext
    {
        public RaidContext(DbContextOptions<RaidContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}