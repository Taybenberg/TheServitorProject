using ActivityDatabase.ORM;

namespace ActivityDatabase
{
    public interface IActivityDB
    {
        IEnumerable<Activity> Activities { get; }

        Task<Activity> GetActivityAsync(ulong activityID);

        Task<Activity> GetActivityWithReservationsAsync(ulong activityID);

        Task AddActivityAsync(Activity activity);

        Task UpdateActivityAsync(Activity activity);

        Task DisableActivityAsync(Activity activity);

        Task DisableActivityAsync(ulong activityID);

        Task<bool> TransferPlaceAsync(ulong activityID, ulong user1ID, ulong user2ID);

        Task<bool> SubscribeUserAsync(ulong activityID, ulong userID);

        Task<bool> UnSubscribeUserAsync(ulong activityID, ulong userId);
    }
}
