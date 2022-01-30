using BungieSharper.Entities.Destiny;

namespace ClanActivitiesService.Containers
{
    public record ClassCounter
    {
        public DestinyClass Class { get; internal set; }

        public int Count { get; internal set; }
    }
}
