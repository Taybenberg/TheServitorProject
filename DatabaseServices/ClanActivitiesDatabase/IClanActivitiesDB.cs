﻿using ClanActivitiesDatabase.ORM;

namespace ClanActivitiesDatabase
{
    public interface IClanActivitiesDB
    {
        Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime? period);

        Task<User> GetUserWithActivitiesAsync(ulong discordID, DateTime? period);

        bool IsDiscordUserRegistered(ulong discordID);

        Task<bool> RegisterUserAsync(long userID, ulong discordID);

        Task<User> GetUserByDiscordIdAsync(ulong discordID);

        Task<User> GetUserWithActivitiesAndOtherUserStatsAsync(ulong discordID);

        Task<IEnumerable<Activity>> GetUserNightfallsAsync(ulong discordID);

        Task<IEnumerable<Activity>> GetUserRaidsAsync(ulong discordID, DateTime afterDate);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<User>> GetUsersWithCharactersAsync();

        Task<IEnumerable<Character>> GetCharactersAsync();

        Task<IEnumerable<ActivityUserStats>> GetActivityUserStatsAsync();

        Task<IEnumerable<Activity>> GetSuspiciousActivitiesWithoutNightfallsAsync();

        Task<IEnumerable<Activity>> GetSuspiciousNightfallsOnlyAsync();
    }
}
