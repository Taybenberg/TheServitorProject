namespace ActivityService
{
    public interface IActivityManager
    {
        public event Func<ActivityContainer, Task> ActivityNotification;
        public event Func<ActivityContainer, Task> ActivityUpdated;
        public event Func<ulong, Task> ActivityDisabled;

        public Task Init();

        Task AddActivityAsync(ActivityContainer activity);

        Task<ActivityContainer> GetActivityAsync(ulong activityID);
    }
}