using System;

namespace web_server.idal.Domain
{
    public class User : DomainBase
    {
        public string Login { get; set; }

        public DateTime CreateTime{ get; set; }

        public string DVRName { get; set; }
    }
}
