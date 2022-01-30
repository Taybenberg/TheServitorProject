namespace ClanActivitiesService.Containers
{
    public class UserActivitiesContainer
    {
        public IEnumerable<ClassCounter> ClassCounters { get; internal set; }

        public ModeCountersContainer ModeCounters { get; internal set; }

        public int TotalCount => ModeCounters.TotalCount;

        public string ChartImageURL => ModeCounters.ChartImageURL;

        public string UserName { get; internal set; }
    }
}
