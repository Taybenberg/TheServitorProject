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

        Task UsersSubscribedAsync(ulong activityID, IEnumerable<ulong> users);

        Task UsersUnSubscribedAsync(ulong activityID, IEnumerable<ulong> users);
    }
}