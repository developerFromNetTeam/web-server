using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface IAuthTokenService
    {
        string GenerateToken();
    }
}
