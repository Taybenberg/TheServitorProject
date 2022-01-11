namespace BungieNetApi
{
    public record ApiKey
    {
        public string Name { get; set; } = "X-API-Key";
        public object Value { get; set; }
    }
}
