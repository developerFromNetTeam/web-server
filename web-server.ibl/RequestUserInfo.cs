using System;

namespace web_server.ibl
{
    public class RequestUserInfo
    {
        public RequestUserInfo(string userId, string login, DateTime createdDate)
        {
            this.UserId = userId;
            this.CreatedDate = createdDate;
            this.Login = login;
        }
        public string UserId { get; private set; }
        public string Login { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
