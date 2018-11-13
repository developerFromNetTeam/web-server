using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using web_server.idal;
using web_server.idal.Domain;

namespace web_server.dal
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase database;

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config["mongodbconnection"]);
            this.database = client.GetDatabase(config["mongodb"]);
        }

        public async Task<IEnumerable<Hashtable>> GetItemsAsync(MongoDbCollection collectionName, BsonDocument filter, string[] fields = null)
        {
            var collection = database.GetCollection<BsonDocument>(collectionName.ToString());
            var cursor = collection.Find(filter);

            List<BsonDocument> result;
            if (fields != null && fields.Any())
            {
                var projections = fields.Select(field => Builders<BsonDocument>.Projection.Include(field)).ToList();
                var combinedProjections = Builders<BsonDocument>.Projection.Combine(projections);

                result = await cursor.Project(combinedProjections).ToListAsync();
            }
            else
            {
                result = await cursor.ToListAsync();
            }
            return result.Select(e => e.ToHashtable());
        }

        public async Task AddItemAsync<T>(MongoDbCollection collectionName, T item) where T : DomainBase
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = Guid.NewGuid().ToString();
            }
            var collection = database.GetCollection<T>(collectionName.ToString());
            await collection.InsertOneAsync(item);
        }

        public async Task DeleteAsync(MongoDbCollection collectionName, BsonDocument filter, bool isManyDelete = false)
        {
            var collection = database.GetCollection<BsonDocument>(collectionName.ToString());
            if (isManyDelete)
            {
                await collection.DeleteManyAsync(filter);
            }
            else
            {
                await collection.DeleteOneAsync(filter);
            }
        }

        public async Task UpdateItemAsync(MongoDbCollection collectionName, BsonDocument filter, BsonDocument updatedFields, bool isManyUpdate = false)
        {
            var collection = database.GetCollection<BsonDocument>(collectionName.ToString());
            if (isManyUpdate)
            {
                await collection.UpdateManyAsync(filter, updatedFields);
            }
            else
            {
                await collection.UpdateOneAsync(filter, updatedFields);
            }
        }
    }
}
