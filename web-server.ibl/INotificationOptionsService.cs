using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface INotificationOptionsService
    {
        Task<IEnumerable<NotificationOptions>> GetOptions(string dvrName);
        Task SetOptions(IEnumerable<NotificationOptions> options, string dvrName);
    }
}
