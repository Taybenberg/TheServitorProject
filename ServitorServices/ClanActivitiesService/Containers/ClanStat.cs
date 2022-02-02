namespace ClanActivitiesService.Containers
{
    public record ClanStat
    {
        public string StatName { get; internal set; }

        public string Value { get; internal set; }
    }
}
