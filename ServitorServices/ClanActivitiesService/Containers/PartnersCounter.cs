namespace ClanActivitiesService.Containers
{
    public record PartnersCounter
    {
        public string UserName { get; internal set; }

        public int Count { get; internal set; }
    }
}
