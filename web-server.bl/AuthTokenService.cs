using System;
using web_server.ibl;

namespace web_server.bl
{
    public class AuthTokenService : IAuthTokenService
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
