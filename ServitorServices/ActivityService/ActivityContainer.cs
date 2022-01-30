using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;

namespace ActivityService
{
    public record ActivityContainer
    {
        public ulong ActivityID { get; set; }

        public ulong ChannelID { get; set; }

        public DateTime PlannedDate { get; set; }

        public DestinyActivityModeType ActivityType { get; set; }

        public string? ActivityName { get; set; }

        public string? Description { get; set; }

        public IEnumerable<ulong> Users { get; set; }
    }
}
