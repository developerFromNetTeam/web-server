using System;
using System.Collections;
using web_server.idal.Domain;

namespace web_server.idal.Converters
{
    public static class SessionConverter
    {
        public static Session ToSession(this Hashtable hashtable)
        {
            return new SessionHistory
            {
                Id = hashtable[MongoDbFields.Id]?.ToString(),
                UserId = hashtable[MongoDbFields.UserId]?.ToString(),
                AuthToken = hashtable[MongoDbFields.AuthToken]?.ToString(),
                City = hashtable[MongoDbFields.City]?.ToString(),
                FcmToken = hashtable[MongoDbFields.FcmToken]?.ToString(),
                IpAddress = hashtable[MongoDbFields.IpAddress]?.ToString(),
                StartUtc = DateTime.Parse(hashtable[MongoDbFields.StartUtc]?.ToString()),
                DvrName = hashtable[MongoDbFields.DVRName]?.ToString(),
            };
        }

        public static SessionHistory ToSessionHistory(this Hashtable hashtable)
        {
            var session = hashtable.ToSession();
            return (SessionHistory)session;
        }

    }
}
