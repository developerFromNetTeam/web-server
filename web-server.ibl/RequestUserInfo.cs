using System;

namespace web_server.ibl
{
    public class RequestUserInfo
    {
        public RequestUserInfo(string userId, string login, DateTime createdDate, string DVRName)
        {
            this.UserId = userId;
            this.CreatedDate = createdDate;
            this.Login = login;
            this.DVRName = DVRName;
        }
        public string UserId { get; private set; }
        public string DVRName { get; private set; }
        public string Login { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
