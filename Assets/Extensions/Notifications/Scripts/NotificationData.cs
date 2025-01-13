namespace MyCore.Features.Notifications
{
    public class NotificationData
    {
        public int id;
        public string title;
        public string body;
        public int hours;
        public int minutes;
        public int seconds;
        public string category;

        public override string ToString()
        {
            var fireTime = System.DateTime.Now.Add(new System.TimeSpan(hours, minutes, seconds));

            var str = "Scheduled Notification: " + "Id: " + id;
            str += "\n Title: " + title;
            str += "\n Body: " + body;
            str += "\n FireTime: " + fireTime;
            str += "\n Category: " + category;

            return str;
        }
    }
}
