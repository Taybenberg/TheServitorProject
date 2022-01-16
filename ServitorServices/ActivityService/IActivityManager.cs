namespace ActivityService
{
    public interface IActivityManager
    {
        public event Func<ActivityContainer, Task> OnNotification;
        public event Func<ActivityContainer, Task> OnUpdated;
        public event Func<ActivityContainer, Task> OnDisabled;

        Task Init();

        Task<ActivityContainer> GetActivityAsync(ulong activityID);

        Task AddActivityAsync(ActivityContainer activity);

        Task UpdateActivityAsync(ActivityContainer activity, ulong callerID);

        Task DisableActivityAsync(ulong activityID);

        Task DisableActivityAsync(ulong activityID, ulong callerID);

        Task RescheduleActivityAsync(ulong activityID, ulong callerID, DateTime plannedDate);

        Task UserTransferPlaceAsync(ulong activityID, ulong userSenderID, ulong userReceiverID);

        Task UserSubscribeOrUnsubscribeAsync(ulong activityID, ulong callerID);

        Task UsersSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users);

        Task UsersUnSubscribeAsync(ulong activityID, ulong callerID, IEnumerable<ulong> users);
    }
}