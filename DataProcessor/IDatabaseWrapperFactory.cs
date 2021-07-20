using DataProcessor.DatabaseWrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IDatabaseWrapperFactory
    {
        Task<ClanActivities> GetClanActivitiesAsync();

        Task<ClanStats> GetClanStatsAsync(string mode);

        Task<IEnumerable<(string, string[])>> GetModesAsync();

        Task<Leaderboard> GetLeaderboardAsync(string mode, ulong discordUserID);

        Task<MyActivities> GetMyActivitiesAsync(ulong discordUserID);

        Task<MyPartners> GetMyPartnersAsync(ulong discordUserID);

        Task<SuspiciousActivities> GetSuspiciousActivitiesAsync(bool isNightfallsOnly);

        Task<WeeklyMilestone> GetWeeklyMilestoneAsync();

        Task<FindUserByName> GetUserWithSimilarUserNameAsync(ulong discordUserID, string discordUserName);

        Task<bool> RegisterUserAsync(long userID, ulong discordUserID);

        Task<MyRaids> GetMyRaidsAsync(ulong discordUserID, DateTime seasonStart, int weekNumber);
    }
}
