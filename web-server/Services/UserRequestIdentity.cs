using System;
using web_server.ibl;
using web_server.IServices;

namespace web_server.Services
{
    public class UserRequestIdentity : IGetUserRequestIdentity, ISetUserRequestIdentity
    {
        private UserIdentity CurrentUser;

        public UserIdentity GetCurrentUser()
        {
            return this.CurrentUser;
        }

        public void SetUser(RequestUserInfo userInfo)
        {
            this.CurrentUser = new UserIdentity(userInfo);
        }
    }
}
