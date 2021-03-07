using BungieNetApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
    public partial class ClanDatabase : DbContext
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

        public bool IsDiscordUserRegistered(ulong discordID) => Users.Any(x => x.DiscordUserID == discordID);

        public async Task<User> GetUserActivitiesAsync(ulong discordID) => await Users.Include(x => x.Characters).ThenInclude(y => y.ActivityUserStats).ThenInclude(a => a.Activity).FirstOrDefaultAsync(z => z.DiscordUserID == discordID);

        public async Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName) => await Users.Where(x => x.UserName.ToLower().Contains(userName)).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime afterDate) => await Activities.Where(x => x.Period >= afterDate && x.ActivityType != ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync(DateTime afterDate) => await Activities.Where(x => x.Period >= afterDate && x.ActivityType == ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();

        public async Task RegisterUserAsync(long userID, ulong discordID)
        {
            var user = Users.Find(userID);

            if (user is not null)
            {
                user.DiscordUserID = discordID;

                Users.Update(user);

                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<(string UserName, int Count)>> GetUserPartnersAsync(ulong discordID)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

            ConcurrentBag<(string userName, int count)> relations = new();

            if (user is not null)
            {
                var acts = await Activities.Include(a => a.ActivityUserStats).ThenInclude(c => c.Character).Where(x => x.ActivityUserStats.Any(y => y.Character.UserID == user.UserID)).ToListAsync();

                var users = Users.Where(x => x.UserID != user.UserID);

                Parallel.ForEach(users, usr =>
                {
                    relations.Add((usr.UserName, acts.Where(x => x.ActivityUserStats.Any(x => x.Character.UserID == usr.UserID)).Count()));
                });
            }

            return relations.Where(x => x.count > 0).OrderByDescending(x => x.count);
        }
    }
}
