using ClanActivitiesService.Containers;

namespace ClanActivitiesService
{
    public interface IClanActivities
    {
        Task<ModeCountersContainer> GetClanActivitiesAsync(DateTime? period = null);

        Task<UserActivitiesContainer> GetUserActivitiesAsync(ulong userID, DateTime? period = null);

        Task<UserPartnersContainer> GetUserPartnersAsync(ulong userID, DateTime? period = null);
    }
}
