namespace BumperService
{
    public interface IBumpManager
    {
        event Func<BumpNotificationContainer, Task> OnNotify;

        DateTime NextBump { get; }

        public Task<DateTime> RegisterBumpAsync(ulong userID);

        public Task SubscribeUserAsync(ulong userID);

        public Task UnSubscribeUserAsync(ulong userID);
    }
}
