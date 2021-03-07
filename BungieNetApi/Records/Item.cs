namespace BungieNetApi
{
    public record Item
    {
        public string ItemName { get; set; }

        public string ItemIcon { get; set; }

        public string ItemTypeAndTier { get; set; }

        public string UniqueLabel { get; set; }
    }
}
