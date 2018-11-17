using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private ICustomLogger<NotificationOptionsController> logger;
        public NotificationOptionsController(ICustomLogger<NotificationOptionsController> logger, INotificationOptionsService notificationOptionsService, IGetUserRequestIdentity getUserRequestIdentity)
        {
            this.notificationOptionsService = notificationOptionsService;
            this.getUserRequestIdentity = getUserRequestIdentity;
            this.logger = logger;
        }

        [Route("load")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                this.logger.LogInformation("GetOptions.Start");
                var options =
                    await this.notificationOptionsService.GetOptions(this.getUserRequestIdentity.GetCurrentUser()
                        .DVRName);
                this.logger.LogInformation($"GetOptions.OK: {JsonConvert.SerializeObject(options)}");
                return Ok(options);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"GetOptions.Exception: {ex.Message}");
                throw;
            }
        }

        [Route("save")]
        public async Task SaveOptions([FromBody]IEnumerable<NotificationOptions> options)
        {
            try
            {
                var dvrname = this.getUserRequestIdentity.GetCurrentUser().DVRName;
                this.logger.LogInformation($"SaveOptions.Start. DvrName: {dvrname}. Options: {JsonConvert.SerializeObject(options)}");
                await this.notificationOptionsService.SetOptions(options, this.getUserRequestIdentity.GetCurrentUser().DVRName);
                this.logger.LogInformation($"SaveOptions.Ok");
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"SaveOptions.Exception: {ex.Message}");
                throw;
            }

        }
    }
}