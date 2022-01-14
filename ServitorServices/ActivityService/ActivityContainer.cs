namespace ActivityService
{
    public record ActivityContainer
    {
        public ulong ActivityID { get; init; }

        public ulong ChannelID { get; set; }

        public DateTime PlannedDate { get; init; }

        public string ActivityType { get; init; }

        public string? Description { get; init; }

        public IEnumerable<ulong> Users { get; init; }
    }
}
