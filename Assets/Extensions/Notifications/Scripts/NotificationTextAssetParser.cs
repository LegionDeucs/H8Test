using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCore.Features.Notifications
{
    public static class NotificationTextAssetParser
    {
        private const char lineSeperator = '\r';
        private const char fieldSeperator = ',';

        public static Dictionary<string, List<NotificationData>> Parse(TextAsset input)
        {
            var categoryNotifications = new Dictionary<string, List<NotificationData>>();
            var lines = input.text.Split(lineSeperator);
            foreach (string line in lines)
            {
                string l = line;
                if (line.StartsWith(lineSeperator))
                    l = line.Substring(1);

                string[] cells = l.Split(fieldSeperator);

                NotificationData n = new NotificationData();
                int.TryParse(cells[0], out n.id);
                n.title = cells[1];
                n.body = cells[2].Replace("\"", "");
                int.TryParse(cells[3], out n.hours);
                int.TryParse(cells[4], out n.minutes);
                int.TryParse(cells[5], out n.seconds);
                n.category = cells[6];

                if (categoryNotifications.ContainsKey(n.category))
                {
                    categoryNotifications[n.category].Add(n);
                }
                else
                {
                    categoryNotifications.Add(n.category, new List<NotificationData>());
                    categoryNotifications[n.category].Add(n);
                }
            }

            return categoryNotifications;
        }
    }
}