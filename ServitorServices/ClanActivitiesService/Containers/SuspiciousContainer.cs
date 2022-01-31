using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;

namespace ClanActivitiesService.Containers
{
    public record SuspiciousContainer
    {
        public IEnumerable<SuspiciousUser> Users { get; internal set; }

        public DestinyActivityModeType ActivityType { get; internal set; }

        public DateTime Period { get; internal set; }

        public string Score { get; internal set; }
    }
}
