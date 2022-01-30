using ClanActivitiesService.Containers;

namespace ClanActivitiesService
{
    public interface IClanActivities
    {
        Task<ModeCountersContainer> GetClanActivitiesAsync(DateTime? period = null);
    }
}
