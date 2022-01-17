namespace DestinyNotificationsDatabase
{
    public class NotificationsUoW : INotificationsDB
    {
        private readonly NotificationsContext _context;

        public NotificationsUoW(NotificationsContext context) => _context = context;
    }
}
