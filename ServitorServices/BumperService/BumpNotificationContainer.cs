namespace BumperService
{
    public record BumpNotificationContainer
    {
        public IEnumerable<ulong> PingableUserIDs { get; internal set; }

        public IDictionary<ulong, DateTime> UserCooldowns { get; internal set; }
    }
}
