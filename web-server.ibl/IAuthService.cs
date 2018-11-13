using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface IAuthService
    {
        Task<string> ValidateAndLoginAsync(string login, string pass, string ip, string city);

        Task LogOutAsync(string userId, bool isAutoEnd = false);

        Task<RequestUserInfo> GetUserInfoByAuthToken(string authToken);
    }
}
