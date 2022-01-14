using ActivityDatabase.ORM;

namespace ActivityService
{
    public interface IActivityManager
    {
        public event Func<Activity, Task> ActivityNotification;
        public event Func<Activity, Task> ActivityUpdated;
        public event Func<ulong, Task> ActivityDeleted;

        public Task Init();
    }
}