using UnityEngine;

namespace MyCore.Features.Notifications
{
    public static class NotificationsController
    {
        private static INotificationsProvider notificationsProvider;


        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
#if UNITY_IOS
            notificationsProvider = new iOSNotificationsProvider();
#elif UNITY_ANDROID
            notificationsProvider = new AndroidNotificationsProvider();
#endif

            if (notificationsProvider == null)
                return;

            notificationsProvider.Init();
            notificationsProvider.CancelAllScheduledNotifications();

            ScheduleDailyNotifications();
        }

        private static void ScheduleDailyNotifications()
        {
            var notificationsCsvFile = Resources.Load<TextAsset>("Notifications");
            var categoryNotifications = NotificationTextAssetParser.Parse(notificationsCsvFile);

            foreach (var kvp in categoryNotifications)
            {
                var notifications = kvp.Value;
                var notificationIndez = Random.Range(0, notifications.Count);
                var notification = notifications[notificationIndez];
                notificationsProvider.ScheduleNotification(notification);
            }
        }
    }
}