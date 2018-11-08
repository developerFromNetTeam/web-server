using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Bson;
using web_server.ibl;
using web_server.idal;
using web_server.idal.Domain;

namespace web_server.bl
{
    public class AuthService : IAuthService
    {
        private IMongoContext mongoContext;
        private ITokenService tokenService;
        public AuthService(IMongoContext mongoContext, ITokenService tokenService)
        {
            this.mongoContext = mongoContext;
            this.tokenService = tokenService;
        }

        public async Task<RequestUserInfo> GetUserInfoByAuthToken(string authToken)
        {
            var sessionItems = await mongoContext.GetItemsAsync(MongoDbCollection.Sessions, new BsonDocument(
                MongoDbQueryOperators.And,
                new BsonArray
                {
                    new BsonDocument(MongoDbFields.AuthToken, authToken),
                    new BsonDocument(MongoDbFields.AuthToken, BsonNull.Value),
                }), new[] { MongoDbFields.UserId });
            var session = sessionItems.FirstOrDefault();
            if (session == null)
            {
                return null;
            }

            var users = await mongoContext.GetItemsAsync(MongoDbCollection.Users,
                new BsonDocument(MongoDbFields.Id, session[MongoDbFields.UserId].ToString()),
                new[] { MongoDbFields.Login, MongoDbFields.CreatedTime });
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return new RequestUserInfo(session[MongoDbFields.UserId].ToString(), user[MongoDbFields.Login].ToString(),
                DateTime.Parse(user[MongoDbFields.CreatedTime].ToString()));
        }

        public async Task LogOutAsync(string authToken, string userId)
        {
            if (string.IsNullOrWhiteSpace(authToken))
            {
                throw new ArgumentNullException(nameof(authToken), "Invalid parameters values.");
            }

            await this.mongoContext.UpdateManyItemAsync(MongoDbCollection.Sessions, new BsonDocument(
                MongoDbQueryOperators.And, new BsonArray
                {
                    new BsonDocument(MongoDbFields.UserId, userId),
                    new BsonDocument(MongoDbFields.AuthToken, authToken),
                    new BsonDocument(MongoDbFields.EndUtc, BsonNull.Value)
                }), new BsonDocument(MongoDbQueryOperators.Set,
                new BsonDocument(MongoDbFields.EndUtc, DateTime.UtcNow)));
        }

        public async Task<string> ValidateAndLoginAsync(string login, string pass, string ip, string city)
        {
            this.ValidateInputs(login, pass, ip, city);
            var items = await this.mongoContext.GetItemsAsync(MongoDbCollection.Users, new BsonDocument(MongoDbQueryOperators.And, new BsonArray
            {
                new BsonDocument(MongoDbFields.Login, login),
                new BsonDocument(MongoDbFields.Password, pass)
            }), new[] { MongoDbFields.Id, MongoDbFields.Login });

            var user = items.FirstOrDefault();
            if (user == null)
            {
                throw new AuthenticationException("Invalid credentials to login.");
            }

            await this.mongoContext.UpdateManyItemAsync(MongoDbCollection.Sessions, new BsonDocument(
                    MongoDbQueryOperators.And,
                    new BsonArray
                    {
                            new BsonDocument(MongoDbFields.UserId, user[MongoDbFields.Id].ToString()),
                            new BsonDocument(MongoDbFields.EndUtc, BsonNull.Value)
                    }),
                new BsonDocument(MongoDbQueryOperators.Set,
                    new BsonDocument(MongoDbFields.EndUtc, DateTime.UtcNow)));

            var usersToken = this.tokenService.GenerateToken();
            await this.mongoContext.AddItemAsync(MongoDbCollection.Sessions, new Session
            {
                UserId = user[MongoDbFields.Id].ToString(),
                AuthToken = usersToken,
                StartUtc = DateTime.UtcNow,
                IpAddress = ip,
                City = city
            });

            return usersToken;
        }

        private void ValidateInputs(string login, string pass, string ip, string city)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login), "Invalid parameters values.");
            }
            if (string.IsNullOrWhiteSpace(pass))
            {
                throw new ArgumentNullException(nameof(pass), "Invalid parameters values.");
            }
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentNullException(nameof(ip), "Invalid parameters values.");
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentNullException(nameof(city), "Invalid parameters values.");
            }
        }
    }
}
