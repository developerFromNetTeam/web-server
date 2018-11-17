using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_server.ibl;
using web_server.IServices;

namespace web_server.Controllers
{
    [Route("api/client-token")]
    public class ClientTokenController : Controller
    {
        private IFcmTokenService fcmTokenService;
        private IGetUserRequestIdentity getUserRequestIdentity;
        private ICustomLogger<ClientTokenController> logger;

        public ClientTokenController(IFcmTokenService fcmTokenService, IGetUserRequestIdentity getUserRequestIdentity, ICustomLogger<ClientTokenController> logger)
        {
            this.fcmTokenService = fcmTokenService;
            this.getUserRequestIdentity = getUserRequestIdentity;
            this.logger = logger;
        }

        // POST api/values
        [HttpPost("set")]
        public async Task<IActionResult> Post([FromBody]string fcmToken)
        {
            try
            {
                this.logger.LogInformation($"SetToken.Start");
                await this.fcmTokenService.UpdateClientTokenAsync(fcmToken, this.getUserRequestIdentity.GetCurrentUser().UserId);
                this.logger.LogInformation($"SetToken.Ok");
                return Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SetToken.Exception: {ex.Message}");
                throw;
            }

        }
    }
}