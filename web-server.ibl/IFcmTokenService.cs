using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface IFcmTokenService
    {
        Task UpdateClientTokenAsync(string token, string userId);
    }
}
