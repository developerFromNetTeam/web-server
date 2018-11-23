using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_server.ibl;
using web_server.IServices;

namespace web_server.Controllers
{
    [Produces("application/json")]
    [Route("api/captured-video")]
    public class CapturedVideoController : Controller
    {
        private IGetUserRequestIdentity getUserRequestIdentity;
        private ICapturedVideoService capturedVideoService;
        private ICustomLogger<ClientTokenController> logger;

        public CapturedVideoController(ICustomLogger<ClientTokenController> logger, IGetUserRequestIdentity getUserRequestIdentity, ICapturedVideoService capturedVideoService)
        {
            this.logger = logger;
            this.getUserRequestIdentity = getUserRequestIdentity;
            this.capturedVideoService = capturedVideoService;
        }

        [Route("load")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var from = DateTime.Parse(HttpContext.Request.Query["from"].ToString());
                var to = DateTime.Parse(HttpContext.Request.Query["to"].ToString());
                var count = int.Parse(HttpContext.Request.Query["count"].ToString());

                var videos = await this.capturedVideoService.GetVideos(from, to,
                    this.getUserRequestIdentity.GetCurrentUser().DVRName, count);
                return Ok(videos);
            }
            catch (FormatException ex)
            {
                this.logger.LogError($"Get.Exception: {ex.Message}");
                return BadRequest("Invalid parameters value(s)");
            }
            catch (ApplicationException ex)
            {
                this.logger.LogError($"Get.Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get.Exception: {ex.Message}");
                throw;
            }
        }
    }
}