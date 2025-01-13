#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;

namespace TFPlay.Features.Notifications
{
    public class AndroidNotificationsProvider : INotificationsProvider
    {
        private const string SCHEDULE_CHANNEL_ID = "schedule_channel_id";

        public void Init()
        {
            SetChannel();
            RemoveNotificationFromStatusBar();

            AndroidNotificationCenter.OnNotificationReceived += AndroidNotificationCenter_OnNotificationReceived;
        }

        public void ScheduleNotification(NotificationData notificationData)
        {
            var notification = new AndroidNotification();
            notification.Title = notificationData.title;
            notification.Text = notificationData.body;
            notification.FireTime = DateTime.Now.AddHours(notificationData.hours);
            notification.FireTime = notification.FireTime.AddMinutes(notificationData.minutes);
            notification.FireTime = notification.FireTime.AddSeconds(notificationData.seconds);
            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, SCHEDULE_CHANNEL_ID, notificationData.id);
        }

        public void CancelAllScheduledNotifications()
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        private void SetChannel()
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = SCHEDULE_CHANNEL_ID,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        private void AndroidNotificationCenter_OnNotificationReceived(AndroidNotificationIntentData notificationIntentData)
        {
            CancelDeliveredNotification(notificationIntentData.Id);
        }

        private void RemoveNotificationFromStatusBar()
        {
            var notification = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notification != null)
            {
                var msg = "Last Received Notification: " + notification.Id;
                msg += "\n - Notification received: ";
                msg += "\n - .Channel ID: " + notification.Channel;
                msg += "\n - .Title: " + notification.Notification.Title;
                msg += "\n - .Text: " + notification.Notification.Text;
                UnityEngine.Debug.Log(msg);

                CancelDeliveredNotification(notification.Id);
            }
        }

        private void CancelDeliveredNotification(int notificationId)
        {
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(notificationId) == NotificationStatus.Delivered)
            {
                AndroidNotificationCenter.CancelNotification(notificationId);
            }
        }
    }
}
#endif