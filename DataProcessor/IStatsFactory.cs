using DataProcessor.DatabaseStats;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IStatsFactory
    {
        Task<ClanActivities> GetClanActivitiesAsync();

        Task<ClanStats> GetClanStatsAsync(string mode);

        Task<IEnumerable<(string, string[])>> GetModesAsync();

        Task<Leaderboard> GetLeaderboardAsync(string mode, ulong discordUserID);

        Task<MyActivities> GetMyActivitiesAsync(ulong discordUserID);

        Task<MyPartners> GetMyPartnersAsync(ulong discordUserID);

        Task<SuspiciousActivities> GetSuspiciousActivitiesAsync(bool isNightfallsOnly);

        Task<WeeklyMilestone> GetWeeklyMilestoneAsync();
    }
}
