using BungieNetApi.Enums;
using ClanActivitiesDatabase.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClanActivitiesDatabase
{
    public partial class ClanActivitiesUoW : IClanActivitiesDB
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ClanActivitiesContext _context;

        public ClanActivitiesUoW(IConfiguration configuration, ILogger<ClanActivitiesUoW> logger, IServiceScopeFactory scopeFactory, ClanActivitiesContext context) =>
            (_configuration, _logger, _scopeFactory, _context) = (configuration, logger, scopeFactory, context);

        public bool IsDiscordUserRegistered(ulong discordID) =>
            _context.Users
            .Any(x => x.DiscordUserID == discordID);

        public async Task<bool> RegisterUserAsync(long userID, ulong discordID)
        {
            var user = _context.Users.Find(userID);

            if (user is null)
                return false;

            user.DiscordUserID = discordID;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByDiscordIdAsync(ulong discordID) =>
            await _context.Users
            .Include(c => c.Characters)
            .FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

        public async Task<User> GetUserWithActivitiesAsync(ulong discordID) =>
            await _context.Users
            .Include(x => x.Characters)
            .ThenInclude(y => y.ActivityUserStats)
            .ThenInclude(a => a.Activity)
            .FirstOrDefaultAsync(z => z.DiscordUserID == discordID);

        public async Task<User> GetUserWithActivitiesAndOtherUserStatsAsync(ulong discordID) =>
            await _context.Users
            .Include(x => x.Characters)
            .ThenInclude(y => y.ActivityUserStats)
            .ThenInclude(a => a.Activity)
            .ThenInclude(u => u.ActivityUserStats)
            .FirstOrDefaultAsync(z => z.DiscordUserID == discordID);

        public async Task<IEnumerable<Activity>> GetUserNightfallsAsync(ulong discordID) =>
            await _context.Activities
            .Include(s => s.ActivityUserStats)
            .Where(x => x.ActivityType == ActivityType.ScoredNightfall &&
            x.ActivityUserStats.Any(y => y.Completed && y.CompletionReasonValue == 0 && y.Character.User.DiscordUserID == discordID))
            .ToListAsync();

        public async Task<IEnumerable<Activity>> GetUserRaidsAsync(ulong discordID, DateTime afterDate) =>
            await _context.Activities
            .Include(s => s.ActivityUserStats)
            .Where(x => x.Period > afterDate && x.ActivityType == ActivityType.Raid &&
            x.ActivityUserStats.Any(y => y.Character.User.DiscordUserID == discordID))
            .ToListAsync();

        public async Task<IEnumerable<User>> GetUsersAsync() =>
            await _context.Users
            .ToListAsync();

        public async Task<IEnumerable<User>> GetUsersWithCharactersAsync() =>
            await _context.Users
            .Include(x => x.Characters)
            .ToListAsync();

        public async Task<IEnumerable<Character>> GetCharactersAsync() =>
            await _context.Characters
            .ToListAsync();

        public async Task<IEnumerable<Activity>> GetActivitiesAsync() =>
            await _context.Activities
            .ToListAsync();

        public async Task<IEnumerable<ActivityUserStats>> GetActivityUserStatsAsync() =>
            await _context.ActivityUserStats
            .ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync() =>
            await _context.Activities
            .Where(x => x.Period > DateTime.Now.AddDays(-7) && x.SuspicionIndex > 0 && x.ActivityType != ActivityType.ScoredNightfall)
            .OrderByDescending(x => x.Period)
            .Take(10)
            .ToListAsync();

        public async Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync() =>
            await _context.Activities
            .Where(x => x.Period > DateTime.Now.AddDays(-7) && x.SuspicionIndex > 0 && x.ActivityType == ActivityType.ScoredNightfall)
            .OrderByDescending(x => x.Period)
            .Take(10)
            .ToListAsync();
    }
}
