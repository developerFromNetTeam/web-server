using System.Threading.Tasks;

namespace webserver.IServices
{
    public interface ITokenService
    {
        Task UpdateClientTokenAsync(string token);
    }
}
