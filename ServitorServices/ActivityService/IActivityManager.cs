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

        Task UpdateActivityAsync(ActivityContainer activity);

        Task DisableActivityAsync(ulong activityID);

        Task RescheduleActivityAsync(ulong activityID, DateTime plannedDate);

        Task UserTransferPlaceAsync(ulong activityID, ulong userSenderID, ulong userReceiverID);

        Task UsersSubscribedAsync(ulong activityID, IEnumerable<ulong> users);

        Task UsersUnSubscribedAsync(ulong activityID, IEnumerable<ulong> users);
    }
}