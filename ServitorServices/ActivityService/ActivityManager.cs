using ActivityDatabase.ORM;
using BungieNetApi.Enums;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ActivityService
{
    public class ActivityManager : IDisposable, IActivityManager
    {
        const int NotifyIntervalMinutes = -10;
        const int DeleteIntervalMinutes = 60;

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly BackgroundJobServer _server;

        public ActivityManager(ILogger<ActivityManager> logger, IServiceScopeFactory scopeFactory)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            GlobalConfiguration.Configuration.UseMemoryStorage();

            _server = new();
        }

        public event Func<ActivityContainer, Task> OnNotification;
        public event Func<ActivityContainer, Task> OnUpdated;
        public event Func<ActivityContainer, Task> OnDisabled;

        private ActivityContainer GetActivityContainer(Activity activity) =>
            new ActivityContainer
            {
                ActivityID = activity.ActivityID,
                ChannelID = activity.ChannelID,
                ActivityType = (ActivityType)activity.ActivityType,
                ActivityName = activity.ActivityName,
                PlannedDate = activity.PlannedDate,
                Description = activity.Description,
                Users = activity.Reservations.OrderBy(x => x.Position).Select(x => x.UserID)
            };

        private Activity GetActivity(ActivityContainer activity) =>
            new Activity
            {
                ActivityID = activity.ActivityID,
                ChannelID = activity.ChannelID,
                ActivityType = (int)activity.ActivityType,
                ActivityName = activity.ActivityName,
                PlannedDate = activity.PlannedDate,
                Description = activity.Description,
                Reservations = activity.Users.Select((u, i) => new Reservation
                {
                    ActivityID = activity.ActivityID,
                    Position = i,
                    UserID = u
                }).ToList()
            };

        public async Task Init()
        {
            _logger.LogInformation($"{DateTime.Now} Init");
            /*
            using var scope = _scopeFactory.CreateScope();

            var currDate = DateTime.UtcNow;

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            foreach (var activity in activityDB.Activities)
            {
                if (activity.PlannedDate < currDate)
                {
                    var tmpAct = await activityDB.DisableActivityAsync(activity);

                    ActivityDisabled?.Invoke(GetActivityContainer(tmpAct));
                }
                else
                    ScheduleActivity(GetActivityContainer(activity));
            }
            */
        }

        public async Task<ActivityContainer> GetActivityAsync(ulong activityID)
        {
            _logger.LogInformation($"{DateTime.Now} Get activity {activityID}");

            /*
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var activity = await activityDB.GetActivityWithReservationsAsync(activityID);

            if (activity is null)
                return null;

            return GetActivityContainer(activity);
            */
            return null;
        }

        public async Task AddActivityAsync(ActivityContainer activity)
        {
            _logger.LogInformation($"{DateTime.Now} Add activity {activity.ActivityID}");

            OnUpdated?.Invoke(activity);

            /*
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            await activityDB.AddActivityAsync(GetActivity(activity));

            ScheduleActivity(activity);

            ActivityUpdated?.Invoke(activity);
            */
        }

        public async Task UpdateActivityAsync(ActivityContainer activity, ulong callerID)
        {
            _logger.LogInformation($"{DateTime.Now} Update activity {activity.ActivityID}");

            /*
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            await activityDB.AddActivityAsync(GetActivity(activity));

            ScheduleActivity(activity);

            ActivityUpdated?.Invoke(activity);
            */
        }

        public async Task DisableActivityAsync(ulong activityID)
        {
            _logger.LogInformation($"{DateTime.Now} Disable activity {activityID}");
        }

        public async Task DisableActivityAsync(ulong activityID, ulong callerID)
        {
            _logger.LogInformation($"{DateTime.Now} Disable activity {activityID}");
        }

        public async Task RescheduleActivityAsync(ulong activityID, ulong callerID, DateTime plannedDate)
        {
            _logger.LogInformation($"{DateTime.Now} Reschedule activity {activityID}");
        }

        public async Task UserTransferPlaceAsync(ulong activityID, ulong userSenderID, ulong userReceiverID)
        {
            _logger.LogInformation($"{DateTime.Now} User transfer {activityID}");
        }

        public async Task UsersSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users)
        {
            _logger.LogInformation($"{DateTime.Now} User subscribed {activityID}");
        }

        public async Task UsersUnSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users)
        {
            _logger.LogInformation($"{DateTime.Now} Users unsubscribed {activityID}");
        }

        public void Dispose() => _server.Dispose();

        private ConcurrentDictionary<ulong, string> _activityJobs = new();
        private void ScheduleActivity(ActivityContainer activity)
        {
            /*
            var notificationDate = activity.PlannedDate.AddMinutes(-10);

            var jobID = BackgroundJob.Schedule(() => ScheduledActivityAsync(activity.ActivityID), activity.PlannedDate);

            _activityJobs.TryAdd(activity.ActivityID, jobID);
            */
        }

        public async Task ScheduledActivityAsync(ulong activityID)
        {
            /*
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var activity = await activityDB.GetActivityWithReservationsAsync(activityID);

            if (activity.IsActive)
                ActivityNotification?.Invoke(GetActivityContainer(activity));
            */
        }

        public async Task DisableActivity(ulong activityID)
        {
            /*
            if (_activityJobs.Remove(activityID, out var jobID))
            {
                using var scope = _scopeFactory.CreateScope();

                var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

                BackgroundJob.Delete(jobID);

                var activity = await activityDB.DisableActivityAsync(activityID);

                ActivityDisabled?.Invoke(GetActivityContainer(activity));
            }
            */
        }
    }
}
