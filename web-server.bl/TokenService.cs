using System;
using web_server.ibl;

namespace web_server.bl
{
    public class TokenService : ITokenService
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
