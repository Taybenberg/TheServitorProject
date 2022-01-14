using ActivityDatabase;
using ActivityDatabase.ORM;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ActivityService
{
    public class ActivityManager : IDisposable, IActivityManager
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly BackgroundJobServer _server;

        public ActivityManager(ILogger<ActivityManager> logger, IServiceScopeFactory scopeFactory)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            GlobalConfiguration.Configuration.UseMemoryStorage();

            _server = new();
        }

        public event Func<ActivityContainer, Task> ActivityNotification;
        public event Func<ActivityContainer, Task> ActivityUpdated;
        public event Func<ulong, Task> ActivityDisabled;

        public async Task Init()
        {
            using var scope = _scopeFactory.CreateScope();

            var currDate = DateTime.UtcNow;

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            foreach (var activity in activityDB.Activities)
            {
                if (activity.PlannedDate < currDate)
                {
                    await activityDB.DisableActivityAsync(activity);

                    ActivityDisabled?.Invoke(activity.ActivityID);
                }
                else
                {
                    //
                }
            }
        }

        public async Task AddActivityAsync(ActivityContainer activity)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            await activityDB.AddActivityAsync(new Activity
            {
                ActivityID = activity.ActivityID,
                ChannelID = activity.ChannelID,
                ActivityType = activity.ActivityType,
                PlannedDate = activity.PlannedDate,
                Description = activity.Description,
                Reservations = activity.Users.Select((u, i) => new Reservation
                {
                    ActivityID = activity.ActivityID,
                    Position = i,
                    UserID = u
                }).ToList()
            });

            //init scheduler

            ActivityUpdated?.Invoke(activity);
        }

        public async Task<ActivityContainer> GetActivityAsync(ulong activityID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var activity = await activityDB.GetActivityWithReservationsAsync(activityID);

            if (activity is null)
                return null;

            return new ActivityContainer
            {
                ActivityID = activity.ActivityID,
                ChannelID = activity.ChannelID,
                ActivityType = activity.ActivityType,
                PlannedDate = activity.PlannedDate,
                Description = activity.Description,
                Users = activity.Reservations.OrderBy(x => x.Position).Select(x => x.UserID)
            };
        }

        public void Dispose() => _server.Dispose();
    }
}
