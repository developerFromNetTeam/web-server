using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using web_server.ibl;
using web_server.idal;
using web_server.idal.Converters;
using web_server.idal.Domain;
using NotificationOptions = web_server.ibl.NotificationOptions;

namespace web_server.bl
{
    public class NotificationOptionsService : INotificationOptionsService
    {
        private IMongoContext mongoContext;

        public NotificationOptionsService(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public async Task SetOptions(IEnumerable<NotificationOptions> options, string dvrName)
        {
            var bsonArrayToUpdate = new BsonArray(options.Select(x => new BsonDocument(new List<BsonElement>(new[]
            {
                new BsonElement(MongoDbFields.CameraSystemName,x.CameraSystemName),
                new BsonElement(MongoDbFields.CameraUserName,x.CameraUserName),
                new BsonElement(MongoDbFields.IsNotificationEnable,x.IsNotificationEnable)
            }))));

            await this.mongoContext.UpdateItemAsync(MongoDbCollection.dvrNotificationOptions,
                new BsonDocument(MongoDbFields.DVRName, dvrName),
                new BsonDocument(MongoDbQueryOperators.Set, new BsonDocument(MongoDbFields.NotificationOptions, bsonArrayToUpdate)));
        }

        public async Task<IEnumerable<ibl.NotificationOptions>> GetOptions(string dvrName)
        {
            var options = await this.mongoContext.GetItemsAsync(MongoDbCollection.dvrNotificationOptions,
                new BsonDocument(MongoDbFields.DVRName, dvrName));
            var notificationOption = options.FirstOrDefault();
            if (notificationOption == null)
            {
                throw new ApplicationException($"Notification options for {dvrName} were not found.");
            }
            return notificationOption.ToCollection(MongoDbFields.NotificationOptions, x => x.ToNotificationOptions())
                .Select(x => new ibl.NotificationOptions
                {
                    CameraSystemName = x.CameraSystemName,
                    IsNotificationEnable = x.IsNotificationEnable,
                    CameraUserName = x.CameraUserName
                }).ToList();
        }
    }
}
