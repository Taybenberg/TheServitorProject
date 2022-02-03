namespace ClanActivitiesService.Containers
{
    public record LeaderboardStat
    {
        public string StatName { get; internal set; }

        public IEnumerable<LeaderboardEntry> Leaders { get; internal set; }
    }
}
