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

        public ClientTokenController(IFcmTokenService fcmTokenService, IGetUserRequestIdentity getUserRequestIdentity)
        {
            this.fcmTokenService = fcmTokenService;
            this.getUserRequestIdentity = getUserRequestIdentity;
        }
        
        // POST api/values
        [HttpPost("set")]
        public async Task<IActionResult> Post([FromBody]string fcmToken)
        {
            await this.fcmTokenService.UpdateClientTokenAsync(fcmToken, this.getUserRequestIdentity.GetCurrentUser().UserId);
            return Ok();
        }
    }
}