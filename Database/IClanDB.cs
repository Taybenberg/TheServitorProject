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

        Task<IEnumerable<Character>> GetCharactersAsync();

        Task<IEnumerable<Activity>> GetActivitiesAsync();

        Task<IEnumerable<ActivityUserStats>> GetActivityUserStatsAsync();

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByDiscordIdAsync(ulong discordID);

        Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName);

        Task<User> GetUserActivitiesAsync(ulong discordID);

        Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime afterDate);

        Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync(DateTime afterDate);

        Task RegisterUserAsync(long userID, ulong discordID);

        Task<IEnumerable<(string UserName, int Count)>> GetUserPartnersAsync(ulong discordID);
    }
}
