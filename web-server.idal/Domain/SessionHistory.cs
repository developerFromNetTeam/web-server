using System;
namespace web_server.idal.Domain
{
    public class SessionHistory : Session
    {
        public DateTime EndUtc { get; set; }
        public bool IsAutoEnd { get; set; }
    }
}
