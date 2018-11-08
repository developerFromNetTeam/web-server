using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using web_server.idal.Domain;

namespace web_server.idal
{
    public interface IMongoContext
    {
        Task<IEnumerable<Hashtable>> GetItemsAsync(MongoDbCollection collectionName, BsonDocument filter, string[] fields = null);
        Task AddItemAsync<T>(MongoDbCollection collectionName, T item) where T : DomainBase;

        Task UpdateManyItemAsync(MongoDbCollection collectionName, BsonDocument filter, BsonDocument updatedFields);
    }
}
