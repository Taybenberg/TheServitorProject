namespace DestinyInfocardsDatabase.ORM.LostSectors
{
    public record LostSector
    {
        public string Name { get; set; }

        public string Reward { get; set; }

        public string LightLevel { get; set; }

        public string ImageURL { get; set; }
    }
}
