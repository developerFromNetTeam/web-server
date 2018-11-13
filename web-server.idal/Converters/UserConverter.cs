using System;
using System.Collections;
using web_server.idal.Domain;

namespace web_server.idal.Converters
{
    public static class UserConverter
    {
        public static User ToUser(this Hashtable hashtable)
        {
            return new User
            {
                Id = hashtable[MongoDbFields.Id].ToString(),
                DVRName = hashtable[MongoDbFields.DVRName].ToString(),
                Login = hashtable[MongoDbFields.Login].ToString(),
                CreateTime = DateTime.Parse(hashtable[MongoDbFields.CreatedTimeUtc].ToString())
            };
        }
    }
}
