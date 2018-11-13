using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_server.ibl;
using web_server.IServices;

namespace web_server.Controllers
{
    [Produces("application/json")]
    [Route("api/notification-options")]
    public class NotificationOptionsController : Controller
    {
        private INotificationOptionsService notificationOptionsService;
        private IGetUserRequestIdentity getUserRequestIdentity;
        public NotificationOptionsController(INotificationOptionsService notificationOptionsService, IGetUserRequestIdentity getUserRequestIdentity)
        {
            this.notificationOptionsService = notificationOptionsService;
            this.getUserRequestIdentity = getUserRequestIdentity;
        }

        [Route("load")]
        public async Task<IActionResult> GetOptions()
        {
            var options = await this.notificationOptionsService.GetOptions(this.getUserRequestIdentity.GetCurrentUser().DVRName);
            return Ok(options);
        }

        [Route("save")]
        public async Task SaveOptions([FromBody]IEnumerable<NotificationOptions> options)
        {
            await this.notificationOptionsService.SetOptions(options, this.getUserRequestIdentity.GetCurrentUser().DVRName);
        }
    }
}