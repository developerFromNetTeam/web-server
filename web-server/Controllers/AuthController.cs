using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using web_server.ibl;
using web_server.IServices;
using web_server.Models;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace web_server.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthService authService;
        private IGetUserRequestIdentity getUserRequestIdentity;
        private ICustomLogger<AuthController> logger;
        public AuthController(IAuthService authService, IGetUserRequestIdentity getUserRequestIdentity, ICustomLogger<AuthController> logger)
        {
            this.authService = authService;
            this.getUserRequestIdentity = getUserRequestIdentity;
            this.logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] AuthDataModel data)
        {
            try
            {
                this.logger.LogInformation($"LogIn: {JsonConvert.SerializeObject(data)}");

                var authToken =
                    await this.authService.ValidateAndLoginAsync(data.Login, data.Pass, data.IpAddress, data.City);

                this.logger.LogInformation($"LogIn.Ok");
                return Ok(authToken);
            }
            catch (AuthenticationException ex)
            {
                this.logger.LogError($"LogIn.AuthenticationException: {ex.Message}");
                return BadRequest("Provided credentials are invalid.");
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError($"LogIn.ArgumentNullException: {ex.Message}");
                return BadRequest("Incorect parameters values.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"LogIn.Exception: {ex.Message}");
                throw;
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var userId = this.getUserRequestIdentity.GetCurrentUser().UserId;
                this.logger.LogInformation($"LogOut: {userId}");
                await this.authService.LogOutAsync(userId);
                this.logger.LogInformation($"LogOut.Ok");
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError($"LogOut.ArgumentNullException: {ex.Message}");
                return BadRequest("Incorect parameters values.");
            }
            catch (ApplicationException ex)
            {
                this.logger.LogError($"LogOut.ApplicationException: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"LogOut.Exception: {ex.Message}");
                throw;
            }
        }
    }
}