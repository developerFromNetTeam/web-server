using System.Threading.Tasks;
using MongoDB.Bson;
using web_server.ibl;
using web_server.idal;

namespace web_server.bl
{
    public class FcmTokenService : IFcmTokenService
    {
        private IMongoContext mongoContext;

        public FcmTokenService(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public async Task UpdateClientTokenAsync(string token, string userId)
        {
            await this.mongoContext.UpdateItemAsync(MongoDbCollection.activeSessions, new BsonDocument(new BsonDocument(MongoDbFields.UserId, userId)),
                new BsonDocument(MongoDbQueryOperators.Set, new BsonDocument(MongoDbFields.FcmToken, token)));
        }
    }
}
