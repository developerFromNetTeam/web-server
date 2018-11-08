using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using webserver.IServices;

namespace webserver.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task UpdateClientTokenAsync(string token)
        {
            using (var writer = File.CreateText(config["tokenFilePath"]))
            {
                await writer.WriteAsync(token);
            }
        }
    }
}
