namespace ClanActivitiesService.Containers
{
    public record SuspiciousUser
    {
        public bool IsClanMember { get; internal set; }

        public string UserName { get; internal set; }

        public string ClanSign { get; internal set; }

        public string ClanName { get; internal set; }
    }
}
