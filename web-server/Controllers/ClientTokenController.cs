using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webserver.IServices;

namespace web_server.Controllers
{
    [Route("api/client-token")]
    public class ClientTokenController : Controller
    {
        private ITokenService tokenService;

        public ClientTokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string token)
        {
            this.tokenService.UpdateClientTokenAsync(token);
            return new StatusCodeResult((int)HttpStatusCode.OK);
        }
    }
}