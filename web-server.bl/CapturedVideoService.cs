using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_server.ibl;
using web_server.idal;
using web_server.idal.Converters;

namespace web_server.bl
{
    public class CapturedVideoService : ICapturedVideoService
    {
        private IMongoContext mongoContext;

        public CapturedVideoService(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public async Task<IEnumerable<CapturedVideoInfo>> GetVideos(DateTime from, DateTime to, string dvrName, int count)
        {
            var videos = await mongoContext.GetItemsAsync(MongoDbCollection.uploadedVideoFiles, new BsonDocument(MongoDbQueryOperators.And, new BsonArray
            {
                new BsonDocument(MongoDbFields.VideoStartDateLocal,new BsonDocument(MongoDbQueryOperators.Gte,from)),
                new BsonDocument(MongoDbFields.VideoStartDateLocal,new BsonDocument(MongoDbQueryOperators.Lte,to)),
                new BsonDocument(MongoDbFields.DVRName, dvrName)
            }));

            if (videos == null || !videos.Any())
            {
                throw new ApplicationException($"Videos for {dvrName} in {from} - {to} were not found.");
            }

            var videosFromDb = videos.TakeLast(count).Select(x => x.ToUploadedVideoFile());
            return videosFromDb.Select(x => new CapturedVideoInfo
            {
                CameraName = x.CameraName,
                Date = x.VideoStartDateLocal,
                UrlPath = x.FilePath
            });
        }
    }
}
