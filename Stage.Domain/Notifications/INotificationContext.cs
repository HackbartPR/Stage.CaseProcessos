using Stage.Domain.Entities;

namespace Stage.Domain.Notifications
{
    public interface INotificationContext
    {
        void AddNotification(string key, string message);

        Dictionary<string, string> Notifications();

        bool HasNotifications();
    }
}
