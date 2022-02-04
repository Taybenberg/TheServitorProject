namespace ClanActivitiesService.Containers
{
    public record RegisterUserContainer
    {
        public bool IsSuccessful { get; internal set; }

        public string UserName { get; internal set; }

        public string Platform { get; internal set; }
    }
}
