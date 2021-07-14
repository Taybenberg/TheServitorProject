using Database.ORM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database
{
    public interface IClanDB
    {
        Task SyncActivitiesAsync();

        Task SyncUsersAsync();

        bool IsDiscordUserRegistered(ulong discordID);

        Task RegisterUserAsync(long userID, ulong discordID);

        Task<User> GetUserByDiscordIdAsync(ulong discordID);

        Task<User> GetUserWithActivitiesAsync(ulong discordID);

        Task<User> GetUserWithActivitiesAndOtherUserStatsAsync(ulong discordID);

        Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<User>> GetUsersWithCharactersAsync();

        Task<IEnumerable<Character>> GetCharactersAsync();

        Task<IEnumerable<Activity>> GetActivitiesAsync();

        Task<IEnumerable<ActivityUserStats>> GetActivityUserStatsAsync();

        Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime afterDate);

        Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync(DateTime afterDate);
    }
}
