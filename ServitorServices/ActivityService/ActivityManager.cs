using ActivityDatabase;
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
                Users = activity.Reservations?.OrderBy(x => x.Position).Select(x => x.UserID) ?? new List<ulong>()
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
                Reservations = activity.Users?.Select((u, i) => new Reservation
                {
                    ActivityID = activity.ActivityID,
                    Position = i,
                    UserID = u
                }).ToList() ?? new List<Reservation>()
            };

        public async Task Init()
        {
            _logger.LogInformation($"{DateTime.Now} ActivityManager initialization…");

            var currDate = DateTime.UtcNow;

            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            foreach (var activity in activityDB.Activities)
            {
                if (activity.PlannedDate < currDate)
                {
                    await activityDB.DisableActivityAsync(activity);

                    OnDisabled?.Invoke(GetActivityContainer(activity));
                }
                else
                    HangfireScheduleActivity(GetActivityContainer(activity));
            }
        }

        public async Task<ActivityContainer> GetActivityAsync(ulong activityID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var activity = await activityDB.GetActivityAsync(activityID);

            return GetActivityContainer(activity);
        }

        public async Task AddActivityAsync(ActivityContainer activity)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            await activityDB.AddActivityAsync(GetActivity(activity));

            _logger.LogInformation($"{DateTime.Now} Added activity {activity.ActivityID}");

            OnUpdated?.Invoke(activity);

            HangfireScheduleActivity(activity);
        }

        public async Task UpdateActivityAsync(ActivityContainer activity, ulong callerID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            if (callerID != activityDB.GetOwnerID(activity.ActivityID))
                return;

            await activityDB.UpdateActivityAsync(GetActivity(activity));

            _logger.LogInformation($"{DateTime.Now} Updated activity {activity.ActivityID}");

            OnUpdated?.Invoke(GetActivityContainer(await activityDB.GetActivityWithReservationsAsync(activity.ActivityID)));
        }

        public async Task DisableActivityAsync(ulong activityID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var activity = await activityDB.DisableActivityAsync(activityID);

            if (activity is not null)
            {
                _logger.LogInformation($"{DateTime.Now} Disabled activity {activityID}");

                var act = GetActivityContainer(await activityDB.GetActivityWithReservationsAsync(activityID));

                OnDisabled?.Invoke(act);

                HangfireUnScheduleActivity(act);
            }
        }

        public async Task DisableActivityAsync(ulong activityID, ulong callerID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            if (callerID != activityDB.GetOwnerID(activityID))
                return;

            await DisableActivityAsync(activityID);
        }

        public async Task RescheduleActivityAsync(ulong activityID, ulong callerID, DateTime plannedDate)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            if (callerID != activityDB.GetOwnerID(activityID))
                return;

            var activity = await activityDB.GetActivityAsync(activityID);

            if (activity is null)
                return;

            activity.PlannedDate = plannedDate;

            await activityDB.UpdateActivityAsync(activity);

            _logger.LogInformation($"{DateTime.Now} Rescheduled activity {activityID}");

            var act = GetActivityContainer(await activityDB.GetActivityWithReservationsAsync(activity.ActivityID));

            OnUpdated?.Invoke(act);

            HangfireReScheduleActivity(act);
        }

        public async Task UserTransferPlaceAsync(ulong activityID, ulong userSenderID, ulong userReceiverID)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            var result = await activityDB.TransferPlaceAsync(activityID, userSenderID, userReceiverID);
            
            if (result)
            {
                _logger.LogInformation($"{DateTime.Now} Transfered activity {activityID} from user {userSenderID} to {userReceiverID}");

                OnUpdated?.Invoke(GetActivityContainer(await activityDB.GetActivityWithReservationsAsync(activityID)));
            }            
        }

        public async Task UsersSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            if (users.Count() == 1 && callerID == users.FirstOrDefault())
                await activityDB.SubscribeUserAsync(activityID, callerID);
            else if (callerID != activityDB.GetOwnerID(activityID))
                return;
            else
                foreach (var user in users)
                    await activityDB.SubscribeUserAsync(activityID, user);

            _logger.LogInformation($"{DateTime.Now} Activity {activityID} subscribed users {string.Join(',', users)}");

            OnUpdated?.Invoke(GetActivityContainer(await activityDB.GetActivityWithReservationsAsync(activityID)));
        }

        public async Task UsersUnSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users)
        {
            using var scope = _scopeFactory.CreateScope();

            var activityDB = scope.ServiceProvider.GetRequiredService<IActivityDB>();

            if (users.Count() == 1 && callerID == users.FirstOrDefault())
                await activityDB.UnSubscribeUserAsync(activityID, callerID);
            else if (callerID != activityDB.GetOwnerID(activityID))
                return;
            else
                foreach (var user in users)
                    await activityDB.UnSubscribeUserAsync(activityID, user);

            _logger.LogInformation($"{DateTime.Now} Activity {activityID} unsubscribed users {string.Join(',', users)}");

            var activity = await activityDB.GetActivityWithReservationsAsync(activityID);

            if (activity.Reservations.Count > 0)
                OnUpdated?.Invoke(GetActivityContainer(activity));
            else
                await DisableActivityAsync(activityID);
        }

        public void Dispose() => _server.Dispose();

        private ConcurrentDictionary<ulong, string> _activityJobs = new();

        private void HangfireScheduleActivity(ActivityContainer activity)
        {
            
        }

        private void HangfireUnScheduleActivity(ActivityContainer activity)
        {

        }

        private void HangfireReScheduleActivity(ActivityContainer activity)
        {

        }
    }
}
