using System.Collections;
using web_server.idal.Domain;

namespace web_server.idal.Converters
{
    public static class NotificationOptionsConverter
    {
        public static NotificationOptions ToNotificationOptions(this Hashtable hashtable)
        {
            return new NotificationOptions
            {
                IsNotificationEnable = bool.Parse(hashtable[MongoDbFields.IsNotificationEnable].ToString()),
                CameraSystemName = hashtable[MongoDbFields.CameraSystemName].ToString(),
                CameraUserName = hashtable[MongoDbFields.CameraUserName].ToString()
            };
        }
    }
}
