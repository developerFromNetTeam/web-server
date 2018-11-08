using System;

namespace web_server.idal.Domain
{
    public class Session : DomainBase
    {
        public string UserId { get; set; }
        public string AuthToken { get; set; }
        public string FcmToken { get; set; }
        public string IpAddress { get; set; }
        public string City { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? EndUtc { get; set; }
    }
}
