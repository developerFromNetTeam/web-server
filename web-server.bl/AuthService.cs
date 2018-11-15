using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Bson;
using web_server.hash_generator;
using web_server.ibl;
using web_server.idal;
using web_server.idal.Converters;
using web_server.idal.Domain;

namespace web_server.bl
{
    public class AuthService : IAuthService
    {
        private IMongoContext mongoContext;
        private IAuthTokenService authTokenService;
        private IMailClient mailClient;
        public AuthService(IMongoContext mongoContext, IAuthTokenService authTokenService, IMailClient mailClient)
        {
            this.mongoContext = mongoContext;
            this.authTokenService = authTokenService;
            this.mailClient = mailClient;
        }

        public async Task<RequestUserInfo> GetUserInfoByAuthToken(string authToken)
        {
            var sessionItems = await mongoContext.GetItemsAsync(MongoDbCollection.activeSessions, new BsonDocument(MongoDbFields.AuthToken, authToken),
                new[] { MongoDbFields.UserId });
            var session = sessionItems.FirstOrDefault();
            if (session == null)
            {
                return null;
            }

            var users = await mongoContext.GetItemsAsync(MongoDbCollection.users, new BsonDocument(MongoDbFields.Id, session[MongoDbFields.UserId].ToString()));
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var userEntity = user.ToUser();
            return new RequestUserInfo(userEntity.Id, userEntity.Login, userEntity.CreateTime, userEntity.DVRName);
        }

        public async Task LogOutAsync(string userId, bool isAutoEnd = false)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId), "Invalid parameters values.");
            }

            var sessions = await this.mongoContext.GetItemsAsync(MongoDbCollection.activeSessions, new BsonDocument(MongoDbFields.UserId, userId));
            var session = sessions.FirstOrDefault();
            if (session == null)
            {
                if (isAutoEnd)
                {
                    return;
                }
                else
                {
                    throw new ApplicationException("Session does not exist.");
                }
            }

            var sessionHistoryEntity = session.ToSessionHistory();

            sessionHistoryEntity.EndUtc = DateTime.UtcNow;
            sessionHistoryEntity.IsAutoEnd = isAutoEnd;

            await this.mongoContext.AddItemAsync(MongoDbCollection.historySessions, sessionHistoryEntity);
            await this.mongoContext.DeleteAsync(MongoDbCollection.activeSessions, new BsonDocument(MongoDbFields.UserId, userId));
        }

        public async Task<string> ValidateAndLoginAsync(string login, string pass, string ip, string city)
        {
            this.ValidateInputs(login, pass, ip, city);
            var items = await this.mongoContext.GetItemsAsync(MongoDbCollection.users, new BsonDocument(MongoDbQueryOperators.And, new BsonArray
            {
                new BsonDocument(MongoDbFields.Login, login),
                new BsonDocument(MongoDbFields.Password, HashGenerator.GenerateHash(pass))
            }), new[] { MongoDbFields.Id, MongoDbFields.Login, MongoDbFields.DVRName });

            var user = items.FirstOrDefault();
            if (user == null)
            {
                throw new AuthenticationException("Invalid credentials to login.");
            }

            try
            {
                await this.mailClient.SendAsync(new MailModel
                {
                    Subject = "New user logged in.",
                    Body = $"Login name: {login}, city: {city}, ip:{ip}"
                });
            }
            catch (Exception ex)
            {

            }

            await this.LogOutAsync(user[MongoDbFields.Id].ToString(), true);

            var usersToken = this.authTokenService.GenerateToken();
            await this.mongoContext.AddItemAsync(MongoDbCollection.activeSessions, new Session
            {
                UserId = user[MongoDbFields.Id].ToString(),
                AuthToken = usersToken,
                StartUtc = DateTime.UtcNow,
                IpAddress = ip,
                City = city,
                DVRName = user[MongoDbFields.DVRName].ToString()
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
