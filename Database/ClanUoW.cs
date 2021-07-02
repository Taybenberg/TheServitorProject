using BungieNetApi.Enums;
using Database.ORM;
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
    public partial class ClanUoW : IClanDB
    {
        private readonly ILogger _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ClanContext _context;

        public ClanUoW(ILogger<ClanUoW> logger, IServiceScopeFactory scopeFactory, ClanContext context)
        {
            _logger = logger;

            _scopeFactory = scopeFactory;

            _context = context;
        }

        public bool IsDiscordUserRegistered(ulong discordID) => _context.Users.Any(x => x.DiscordUserID == discordID);

        public async Task<User> GetUserByDiscordId(ulong discordID) => await _context.Users.FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

        public async Task<User> GetUserActivitiesAsync(ulong discordID) => await _context.Users.Include(x => x.Characters).ThenInclude(y => y.ActivityUserStats).ThenInclude(a => a.Activity).FirstOrDefaultAsync(z => z.DiscordUserID == discordID);

        public async Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName) => await _context.Users.Where(x => x.UserName.ToLower().Contains(userName)).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime afterDate) => await _context.Activities.Where(x => x.Period >= afterDate && x.ActivityType != ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync(DateTime afterDate) => await _context.Activities.Where(x => x.Period >= afterDate && x.ActivityType == ActivityType.ScoredNightfall && x.SuspicionIndex > 0).ToListAsync();

        public async Task<IEnumerable<User>> GetUsersAsync() => await _context.Users.ToListAsync();

        public async Task<IEnumerable<Character>> GetCharactersAsync() => await _context.Characters.ToListAsync();

        public async Task<IEnumerable<Activity>> GetActivitiesAsync() => await _context.Activities.ToListAsync();

        public async Task<IEnumerable<ActivityUserStats>> GetActivityUserStatsAsync() => await _context.ActivityUserStats.ToListAsync();

        public async Task RegisterUserAsync(long userID, ulong discordID)
        {
            var user = _context.Users.Find(userID);

            if (user is not null)
            {
                user.DiscordUserID = discordID;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<(string UserName, int Count)>> GetUserPartnersAsync(ulong discordID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

            ConcurrentBag<(string userName, int count)> relations = new();

            if (user is not null)
            {
                var acts = await _context.Activities.Include(a => a.ActivityUserStats).ThenInclude(c => c.Character).Where(x => x.ActivityUserStats.Any(y => y.Character.UserID == user.UserID)).ToListAsync();

                var users = _context.Users.Where(x => x.UserID != user.UserID);

                Parallel.ForEach(users, usr =>
                {
                    relations.Add((usr.UserName, acts.Where(x => x.ActivityUserStats.Any(x => x.Character.UserID == usr.UserID)).Count()));
                });
            }

            return relations.Where(x => x.count > 0).OrderByDescending(x => x.count);
        }
    }
}
