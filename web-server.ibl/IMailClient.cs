using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface IMailClient
    {
        Task SendAsync(MailModel model);
    }
}
