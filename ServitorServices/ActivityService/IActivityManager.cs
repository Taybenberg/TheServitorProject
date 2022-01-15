namespace ActivityService
{
    public interface IActivityManager
    {
        public event Func<ActivityContainer, Task> ActivityNotification;
        public event Func<ActivityContainer, Task> ActivityUpdated;
        public event Func<ActivityContainer, Task> ActivityDisabled;

        Task Init();

        Task<ActivityContainer> GetActivityAsync(ulong activityID);

        Task AddActivityAsync(ActivityContainer activity);

        Task UpdateActivityAsync(ActivityContainer activity);

        Task DisableActivityAsync(ulong activityID);

        Task UsersSubscribedAsync(ulong activityID, IEnumerable<ulong> users);

        Task UsersUnSubscribedAsync(ulong activityID, IEnumerable<ulong> users);
    }
}