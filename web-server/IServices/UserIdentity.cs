using System;
using web_server.ibl;

namespace web_server.IServices
{
    public class UserIdentity
    {
        public UserIdentity(RequestUserInfo info)
        {
            this.UserId = info.UserId;
            this.CreatedDate = info.CreatedDate;
            this.Login = info.Login;
        }
        public string UserId { get; private set; }
        public string Login { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}