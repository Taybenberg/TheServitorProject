using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesService.Containers;

namespace ClanActivitiesService
{
    public interface IClanActivities
    {
        Task SyncDatabaseAsync();

        Task<ModeCountersContainer> GetClanActivitiesAsync(DateTime? period = null);

        Task<UserActivitiesContainer> GetUserActivitiesAsync(ulong userID, DateTime? period = null);

        Task<UserPartnersContainer> GetUserPartnersAsync(ulong userID, DateTime? period = null);

        Task<IEnumerable<SuspiciousContainer>> GetSuspiciousActivitiesAsync(DestinyActivityModeType? activityType = null);

        Task<IEnumerable<ClanStat>> GetClanStatsAsync(ulong userID, DestinyActivityModeType activityType);

        Task<LeaderboardContainer> GetLeaderboardAsync(ulong userID, DestinyActivityModeType activityType);

        Task<RegisterUserContainer> TryRegisterUserAsync(ulong userID, string userName);
    }
}
