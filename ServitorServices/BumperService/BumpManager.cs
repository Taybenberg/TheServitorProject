using BumperDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BumperService
{
    public class BumpManager : IBumpManager
    {
        const int userBumpCooldownHours = 24;
        const int bumpCooldownHours = 4;

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public BumpManager(ILogger<BumpManager> logger, IServiceScopeFactory scopeFactory)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            var nextBump = NextBump;

            _timer.AutoReset = false;
            _timer.Interval = (nextBump - DateTime.UtcNow).TotalMilliseconds;

            _timer.Elapsed += async (_, _) =>
            {
                _timer.Stop();

                using var scope = _scopeFactory.CreateScope();

                var bumperDB = scope.ServiceProvider.GetRequiredService<IBumperDB>();

                var bumps = await bumperDB.GetLastBumpsAsync(DateTime.UtcNow.AddHours(-userBumpCooldownHours));
                var bumpCooldowns = bumps.ToDictionary(x => x.UserID, x => x.BumpTime.AddHours(userBumpCooldownHours).ToLocalTime());

                var pingUsers = bumperDB.PingableUserIDs.Where(x => !bumpCooldowns.ContainsKey(x));

                Notify?.Invoke(new BumpNotificationContainer
                {
                    UserCooldowns = bumpCooldowns,
                    PingableUserIDs = pingUsers
                });

                _logger.LogInformation($"{DateTime.Now} Bump notification");
            };

            _timer.Start();

            _logger.LogInformation($"{DateTime.Now} Bump scheduled on {nextBump}");
        }

        System.Timers.Timer _timer = new();

        public event Func<BumpNotificationContainer, Task> Notify;

        public DateTime NextBump
        {
            get
            {
                var currTime = DateTime.UtcNow;

                using var scope = _scopeFactory.CreateScope();

                var bumperDB = scope.ServiceProvider.GetRequiredService<IBumperDB>();

                var lastBumpTime = bumperDB.LastBump?.BumpTime;

                if (lastBumpTime is not null)
                {
                    var predictedBump = lastBumpTime.Value.AddHours(bumpCooldownHours);

                    if (currTime < predictedBump)
                        return predictedBump;
                }

                return currTime.AddHours(bumpCooldownHours).ToLocalTime();
            }
        }

        public async Task<DateTime> RegisterBumpAsync(ulong userID)
        {
            var currDate = DateTime.UtcNow;
            var nextBump = currDate.AddHours(bumpCooldownHours);

            using var scope = _scopeFactory.CreateScope();

            var bumperDB = scope.ServiceProvider.GetRequiredService<IBumperDB>();

            await bumperDB.AddBumpAsync(currDate, userID);

            _timer.Stop();

            _timer.Interval = (nextBump - currDate).TotalMilliseconds;

            _timer.Start();

            _logger.LogInformation($"{DateTime.Now} Bump scheduled on {nextBump.ToLocalTime()}");

            return nextBump;
        }

        public async Task SubscribeUserAsync(ulong userID)
        {
            using var scope = _scopeFactory.CreateScope();

            var bumperDB = scope.ServiceProvider.GetRequiredService<IBumperDB>();

            await bumperDB.AddOrUpdateUserAsync(userID, true);

            _logger.LogInformation($"{DateTime.Now} User {userID} subscribed");
        }

        public async Task UnSubscribeUserAsync(ulong userID)
        {
            using var scope = _scopeFactory.CreateScope();

            var bumperDB = scope.ServiceProvider.GetRequiredService<IBumperDB>();

            await bumperDB.AddOrUpdateUserAsync(userID, false);

            _logger.LogInformation($"{DateTime.Now} User {userID} unsubscribed");
        }
    }
}