using System.IO;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ItakuDesktop.Tools
{
    public static class ItakuScraper
    {
        public const string notificationAPI = "https://itaku.ee/api/notifications/get_bulk_unread_notifs/";

        public static ItakuHTTPClient GetNotificationClient()
        {
            var container = GetCookieContainer();
            var authorizationToken = GetToken();
            var client = new ItakuHTTPClient(notificationAPI, authorizationToken, container);
            return client;
        }

        public static void SaveCookieContainer(CookieContainer cookieContainer)
        {
            var json = JsonConvert.SerializeObject(cookieContainer, Formatting.Indented);
            File.WriteAllText(Path.Combine(MainForm.self.ProfilePath, "cookies.json"), json);
        }

        public static CookieContainer GetCookieContainer()
        {
            var path = Path.Combine(MainForm.self.ProfilePath, "cookies.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                CookieContainer cookieContainer = JsonConvert.DeserializeObject<CookieContainer>(json);
            }
            return null;
        }

        public static void SaveToken(string token)
        {
            File.WriteAllText(Path.Combine(MainForm.self.ProfilePath, "token.ini"), token);
        }

        public static string GetToken()
        {
            var path = Path.Combine(MainForm.self.ProfilePath, "token.ini");
            if (File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }

        public static UnreadNotifications FromStringToNotification(string data)
        {
            return JsonConvert.DeserializeObject<UnreadNotifications>(data);
        }
    }

    public class Notifications
    {
        public int all { get; set; }
        public int stars { get; set; }
        public int comments { get; set; }
        public int mentions { get; set; }
        public int other { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Notifications notif)
            {
                if (stars != notif.stars) return false;
                if (comments != notif.comments) return false;
                if (mentions != notif.mentions) return false;
                if (other != notif.other) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class UnreadNotifications
    {
        public Notifications notifications { get; set; } = new Notifications();
        public int messages { get; set; }
        public int commission_requests { get; set; }
        public int unread_submissions { get; set; }
        public int tag_suggestions { get; set; }

        public bool IsThereNewNotifications()
        {
            if (messages > 0) return true;
            if (commission_requests > 0) return true;
            if (unread_submissions > 0) return true;
            if (tag_suggestions > 0) return true;
            if (notifications.all > 0) return true;
            return false;
        }

        public string GetNotificationText()
        {
            var list = new List<string>();
            if (messages > 0) list.Add($"{messages} message(s)");
            if (commission_requests > 0) list.Add($"{messages} request(s)");
            if (unread_submissions > 0) list.Add($"{unread_submissions} submission(s)");
            if (tag_suggestions > 0) list.Add($"{tag_suggestions} tag suggestion(s)");
            if (notifications.comments > 0) list.Add($"{tag_suggestions} comment(s)");
            if (notifications.stars > 0) list.Add($"{tag_suggestions} star(s)");
            if (notifications.mentions > 0) list.Add($"{tag_suggestions} mention(s)");
            if (notifications.other > 0) list.Add((list.Count > 0 ? "and " : null) + $"{tag_suggestions} other(s)");
            return list.Count > 0 ? string.Join(", ", list) : null;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UnreadNotifications notif)
            {
                if (messages != notif.messages) return false;
                if (commission_requests != notif.commission_requests) return false;
                if (unread_submissions != notif.unread_submissions) return false;
                if (tag_suggestions != notif.tag_suggestions) return false;
                if (!notifications.Equals(notif.notifications)) return false;
                return true;
            }
            return false;
        }
    }
}
