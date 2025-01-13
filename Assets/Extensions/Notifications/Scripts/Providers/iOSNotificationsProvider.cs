#if UNITY_IOS
using System;
using Unity.Notifications.iOS;

namespace TFPlay.Features.Notifications
{
    public class iOSNotificationsProvider : INotificationsProvider
    {
        public void Init() 
        {
            iOSNotificationCenter.OnNotificationReceived += IOSNotificationCenter_OnNotificationReceived;
        }

        public void ScheduleNotification(NotificationData notificationData)
        {
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(notificationData.hours, notificationData.minutes, notificationData.seconds),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                Identifier = notificationData.id.ToString(),
                Title = notificationData.title,
                Body = notificationData.body,
                Subtitle = null,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "Category_1",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void CancelAllScheduledNotifications()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        private void IOSNotificationCenter_OnNotificationReceived(iOSNotification delivetedNotification)
        {
            CancelDeliveredNotification(delivetedNotification.Identifier);
        }

        private void RemoveNotificationFromStatusBar()
        {
            var notification = iOSNotificationCenter.GetLastRespondedNotification();
            if (notification != null)
            {
                var msg = "Last Received Notification: " + notification.Identifier;
                msg += "\n - Notification received: ";
                msg += "\n - .Title: " + notification.Title;
                msg += "\n - .Badge: " + notification.Badge;
                msg += "\n - .Body: " + notification.Body;
                msg += "\n - .CategoryIdentifier: " + notification.CategoryIdentifier;
                msg += "\n - .Subtitle: " + notification.Subtitle;
                msg += "\n - .Data: " + notification.Data;
                UnityEngine.Debug.Log(msg);

                CancelDeliveredNotification(notification.Identifier);
            }
        }

        private void CancelDeliveredNotification(string notificationId)
        {
            foreach (var notification in iOSNotificationCenter.GetDeliveredNotifications())
            {
                if (notification.Identifier == notificationId)
                {
                    iOSNotificationCenter.RemoveDeliveredNotification(notificationId);
                }
            }
        }
    }
}
#endif