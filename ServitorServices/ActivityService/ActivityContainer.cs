using BungieNetApi.Enums;

namespace ActivityService
{
    public record ActivityContainer
    {
        public ulong ActivityID { get; set; }

        public ulong ChannelID { get; set; }

        public DateTime PlannedDate { get; set; }

        public ActivityType ActivityType { get; set; }

        public string? ActivityName { get; set; }

        public string? Description { get; set; }

        public IEnumerable<ulong> Users { get; set; }
    }
}
