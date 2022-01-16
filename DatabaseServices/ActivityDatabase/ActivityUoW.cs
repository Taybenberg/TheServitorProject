using ActivityDatabase.ORM;
using Microsoft.EntityFrameworkCore;

namespace ActivityDatabase
{
    public class ActivityUoW : IActivityDB
    {
        private readonly ActivityContext _context;

        public ActivityUoW(ActivityContext context) => _context = context;

        public IEnumerable<Activity> Activities => 
            _context.Activities.Where(x => x.IsActive);

        public ulong? GetOwnerID(ulong activityID) =>
            _context.Reservations.FirstOrDefault(x => x.ActivityID == activityID && x.Position == 0)?.UserID;

        public async Task<int> GetSubscribersCountAsync(ulong ActivityID) =>
            await _context.Reservations.CountAsync(x => x.ActivityID == ActivityID);

        public async Task<Activity> GetActivityAsync(ulong activityID) =>
            await _context.Activities.FindAsync(activityID);

        public async Task<Activity> GetActivityWithReservationsAsync(ulong activityID) =>
            await _context.Activities.Include(x => x.Reservations).FirstOrDefaultAsync(x => x.ActivityID == activityID);

        public async Task AddActivityAsync(Activity activity)
        {
            activity.IsActive = true;

            await _context.Activities.AddAsync(activity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateActivityAsync(Activity activity)
        {
            var dbActivity = await GetActivityAsync(activity.ActivityID);

            if (dbActivity is not null)
            {
                dbActivity.PlannedDate = activity.PlannedDate;
                dbActivity.Description = activity.Description;
                dbActivity.ActivityName = activity.ActivityName;
                dbActivity.ActivityType = activity.ActivityType;

                _context.Activities.Update(dbActivity);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DisableActivityAsync(Activity activity)
        {   
            activity.IsActive = false;

            _context.Activities.Update(activity);

            await _context.SaveChangesAsync();
        }

        public async Task<Activity> DisableActivityAsync(ulong activityID)
        {
            var dbActivity = await GetActivityAsync(activityID);

            if (dbActivity is not null)
            {
                if (!dbActivity.IsActive)
                    return null;

                await DisableActivityAsync(dbActivity);
            }
                
            return dbActivity;
        }

        public async Task<bool> TransferPlaceAsync(ulong activityID, ulong senderID, ulong receiverID)
        {
            var dbActivity = await GetActivityWithReservationsAsync(activityID);

            if (dbActivity is not null)
            {
                var sender = dbActivity.Reservations.FirstOrDefault(x => x.ActivityID == activityID && x.UserID == senderID);
                var receiver = dbActivity.Reservations.FirstOrDefault(x => x.ActivityID == activityID && x.UserID == receiverID);

                if (sender is not null)
                {
                    if (receiver is not null)
                    {
                        if (receiver.Position > sender.Position)
                        {
                            sender.UserID = receiverID;
                            receiver.UserID = senderID;

                            _context.Reservations.Update(sender);
                            _context.Reservations.Update(receiver);
                        }
                    }
                    else
                    {
                        sender.UserID = receiverID;

                        _context.Reservations.Update(sender);
                    }

                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<bool?> SubscribeOrUnsubscribeUserAsync(ulong activityID, ulong userID, bool subscribe, bool unsubscribe)
        {
            var dbActivity = await GetActivityWithReservationsAsync(activityID);

            if (dbActivity is not null)
            {
                var reservation = dbActivity.Reservations.FirstOrDefault(x => x.ActivityID == activityID && x.UserID == userID);

                if (reservation is null && subscribe)
                {
                    var position = dbActivity.Reservations.Any() ?
                    dbActivity.Reservations.Max(x => x.Position) + 1 : 0;

                    await _context.Reservations.AddAsync(
                        new Reservation
                        {
                            ActivityID = activityID,
                            UserID = userID,
                            Position = position
                        });

                    await _context.SaveChangesAsync();

                    return true;
                }
                else if (reservation is not null && unsubscribe)
                {
                    var otherReservations = dbActivity.Reservations.Where(x => x.Position > reservation.Position);

                    foreach (var r in otherReservations)
                        r.Position--;

                    _context.Reservations.Remove(reservation);

                    _context.Reservations.UpdateRange(otherReservations);

                    await _context.SaveChangesAsync();

                    return false;
                }
            }

            return null;
        }
    }
}
