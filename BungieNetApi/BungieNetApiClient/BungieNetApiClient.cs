namespace BungieNetApi
{
    /// <summary>
    /// https://bungie-net.github.io/multi/index.html
    /// </summary>
    internal partial class BungieNetApiClient
    {
        public const string BUNGIE_NET_URL = "https://bungie.net";

        private readonly ApiKey _xApiKey;

        public BungieNetApiClient(string apiKey) => _xApiKey = new ApiKey
        {
            Value = apiKey
        };
    }
}