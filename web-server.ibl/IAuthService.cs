using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface IAuthService
    {
        Task<string> ValidateAndLoginAsync(string login, string pass, string ip, string city);

        Task LogOutAsync(string authToken, string userId);

        Task<RequestUserInfo> GetUserInfoByAuthToken(string authToken);
    }
}
