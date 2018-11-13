using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_server.ibl;
using web_server.IServices;
using web_server.Models;

namespace web_server.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthService authService;
        private IGetUserRequestIdentity getUserRequestIdentity;
        public AuthController(IAuthService authService, IGetUserRequestIdentity getUserRequestIdentity)
        {
            this.authService = authService;
            this.getUserRequestIdentity = getUserRequestIdentity;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] AuthDataModel data)
        {
            try
            {
                var authToken =
                    await this.authService.ValidateAndLoginAsync(data.Login, data.Pass, data.IpAddress, data.City);
                return Ok(authToken);
            }
            catch (AuthenticationException)
            {
                return BadRequest("Provided credentials are invalid.");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Incorect parameters values.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await this.authService.LogOutAsync(this.getUserRequestIdentity.GetCurrentUser().UserId);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Incorect parameters values.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}