using ClanActivitiesDatabase.ORM;
using Microsoft.EntityFrameworkCore;

namespace ClanActivitiesDatabase
{
    public partial class ClanActivitiesUoW : IClanActivitiesDB
    {
        private readonly ClanActivitiesContext _context;

        public ClanActivitiesUoW(ClanActivitiesContext context) => _context = context;

        public async Task<IEnumerable<User>> GetUsersAsync() =>
            await _context.Users
                .ToListAsync();

        public async Task<IEnumerable<User>> GetUsersWithCharactersAsync() =>
            await _context.Users
                .Include(x => x.Characters)
                .ToListAsync();

        public async Task<IEnumerable<User>> GetUsersWithCharactersAndLastActivityAsync() =>
           await _context.Users
               .Include(x => x.Characters)
               .ThenInclude(x => x.ActivityUserStats.OrderByDescending(y => y.Activity.Period).Take(1))
               .ThenInclude(x => x.Activity)
               .ToListAsync();

        public async Task<IEnumerable<Character>> GetCharactersAsync() =>
            await _context.Characters
                .ToListAsync();

        public async Task<IEnumerable<Character>> GetCharactersWithUsersAsync() =>
            await _context.Characters
                .Include(x => x.User)
                .ToListAsync();

        public async Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime? period) =>
            period is null ?
            await _context.Activities
                .ToListAsync() :
            await _context.Activities
                .Where(x => x.Period > period)
                .ToListAsync();

        public async Task<User?> GetUserWithActivitiesAsync(ulong discordID, DateTime? period) =>
            period is null ?
            await _context.Users
                .Include(x => x.Characters)
                .ThenInclude(x => x.ActivityUserStats)
                .ThenInclude(x => x.Activity)
                .FirstOrDefaultAsync(x => x.DiscordUserID == discordID) :
            await _context.Users
                .Include(x => x.Characters)
                .ThenInclude(x => x.ActivityUserStats.Where(y => y.Activity.Period > period))
                .ThenInclude(x => x.Activity)
                .FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

        public async Task<User?> GetUserWithActivitiesAndOtherUserStatsAsync(ulong discordID, DateTime? period) =>
            period is null ?
            await _context.Users
                .Include(x => x.Characters)
                .ThenInclude(x => x.ActivityUserStats)
                .ThenInclude(x => x.Activity)
                .ThenInclude(x => x.ActivityUserStats)
                .FirstOrDefaultAsync(x => x.DiscordUserID == discordID) :
            await _context.Users
                .Include(x => x.Characters)
                .ThenInclude(x => x.ActivityUserStats.Where(y => y.Activity.Period > period))
                .ThenInclude(x => x.Activity)
                .ThenInclude(x => x.ActivityUserStats)
                .FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

        public async Task<IEnumerable<Activity>> GetSuspiciousActivitiesAsync(int? activityType, DateTime period) =>
            activityType is null ?
            await _context.Activities
                .Where(x => x.SuspicionIndex > 0 && x.Period > period)
                .ToListAsync() :
            await _context.Activities
                .Where(x => x.SuspicionIndex > 0 && x.Period > period && x.ActivityType == activityType)
                .ToListAsync();

        public bool IsDiscordUserRegistered(ulong discordID) =>
            _context.Users
                .Any(x => x.DiscordUserID == discordID);

        public async Task<User?> GetUserByDiscordIdAsync(ulong discordID) =>
            await _context.Users
                .FirstOrDefaultAsync(x => x.DiscordUserID == discordID);

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

        public async Task SyncUsersAsync(IEnumerable<User>? usersToDelete, IEnumerable<User>? usersToUpdate, IEnumerable<User>? usersToAdd)
        {
            if (usersToDelete is not null)
                _context.Users.RemoveRange(usersToDelete);

            if (usersToUpdate is not null)
                _context.Users.UpdateRange(usersToUpdate);

            if (usersToAdd is not null)
                _context.Users.AddRange(usersToAdd);    

            await _context.SaveChangesAsync();
        }

        public async Task SyncActivitiesAsync(IEnumerable<Activity>? activitiesToDelete, IEnumerable<Activity>? activitiesToUpdate, IEnumerable<Activity>? activitiesToAdd)
        {
            if (activitiesToDelete is not null)
                _context.Activities.RemoveRange(activitiesToDelete);

            if (activitiesToUpdate is not null)
                _context.Activities.UpdateRange(activitiesToUpdate);

            if (activitiesToAdd is not null)
                _context.Activities.AddRange(activitiesToAdd);

            await _context.SaveChangesAsync();
        }

        /*
        public async Task<IEnumerable<Activity>> GetUserNightfallsAsync(ulong discordID) =>
            await _context.Activities
            .Include(s => s.ActivityUserStats)
            //.Where(x => x.ActivityType == ActivityType.ScoredNightfall &&
            //x.ActivityUserStats.Any(y => y.Completed && y.CompletionReasonValue == 0 && y.Character.User.DiscordUserID == discordID))
            .ToListAsync();

        public async Task<IEnumerable<Activity>> GetUserRaidsAsync(ulong discordID, DateTime afterDate) =>
            await _context.Activities
            .Include(s => s.ActivityUserStats)
            //.Where(x => x.Period > afterDate && x.ActivityType == ActivityType.Raid &&
            //x.ActivityUserStats.Any(y => y.Character.User.DiscordUserID == discordID))
            .ToListAsync();
        */
    }
}
