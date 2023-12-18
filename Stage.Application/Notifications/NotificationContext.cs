using Stage.Domain.Entities;
using Stage.Domain.Notifications;

namespace Stage.Application.Notifications
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications;
        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public bool HasNotifications()
        {
            return _notifications.Any();
        }

        public Dictionary<string, string> Notifications()
        {
            return _notifications.ToDictionary(e => e.Error, e => e.Message); ;
        }
    }
}
