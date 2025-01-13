using System;

namespace MyCore.Features.Notifications
{
    public interface INotificationsProvider
    {
        void Init();
        void ScheduleNotification(NotificationData notificationData);
        void CancelAllScheduledNotifications();
    }
}