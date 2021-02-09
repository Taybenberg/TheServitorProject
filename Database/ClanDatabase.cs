using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BungieNetApi;

namespace Database
{
    public partial class ClanDatabase : DbContext
    {
        private IConfiguration _configuration;
        private ILogger _logger;

        private BungieNetApiClient _apiClient;

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityUserStats> ActivityUserStats { get; set; }
        public DbSet<UserRelations> UserRelations { get; set; }

        public ClanDatabase(IConfiguration configuration, ILogger<ClanDatabase> logger) : base()
        {
            _configuration = configuration;
            _logger = logger;

            _apiClient = new BungieNetApiClient(configuration);

            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => //options.UseSqlite(@"Data Source=mydb.db;");
        options.UseSqlServer(_configuration.GetConnectionString("ClanDatabase"));

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesAsync(DateTime afterDate) => await Activities.Where(x => x.Period >= afterDate && x.SuspicionIndex > 0).ToListAsync();
    }
}
