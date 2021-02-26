using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BungieNetApi;

namespace Database
{
    public partial class ClanDatabase : DbContext, IDisposable
    {
        private readonly ILogger _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityUserStats> ActivityUserStats { get; set; }

        public ClanDatabase(ILogger<ClanDatabase> logger, IServiceScopeFactory scopeFactory, DbContextOptions<ClanDatabase> options) : base(options)
        {
            _logger = logger;

            _scopeFactory = scopeFactory;

            Database.EnsureCreated();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public bool IsDiscordUserRegistered(ulong discordID) => Users.Any(x => x.DiscordUserID == discordID);

        public async Task<User> GetUserByDiscordIdAsync(ulong discordID) => await Users.FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

        public async Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName) => await Users.Where(x => x.UserName.ToLower().Contains(userName)).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime afterDate) => await Activities.Where(x => x.Period >= afterDate && x.ActivityType != ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync(DateTime afterDate) => await Activities.Where(x => x.Period >= afterDate && x.ActivityType == ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();
    }
}
