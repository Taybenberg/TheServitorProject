using BungieSharper.Entities.Destiny;

namespace ClanActivitiesService.Containers
{
    public record LeaderboardEntry
    {
        public bool IsCurrUser { get; internal set; }

        public int Rank { get; internal set; }

        public DestinyClass DestinyClass { get; internal set; }

        public string UserName { get; internal set; }

        public string Value { get; internal set; }
    }
}
