using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;

namespace ClanActivitiesService.Containers
{
    public record ModeCounter
    {
        public DestinyActivityModeType ActivityMode { get; internal set; }

        public int Count { get; internal set; }
    }
}
