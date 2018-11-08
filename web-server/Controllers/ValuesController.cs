using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webserver.IServices;

namespace web_server.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ILogger<ValuesController> logger;
        private IEventIdGenerator eventIdGenerator;
        public ValuesController(ILogger<ValuesController> logger, IEventIdGenerator eventIdGenerator)
        {
            this.logger = logger;
            this.eventIdGenerator = eventIdGenerator;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            logger.LogInformation("Get value:{value}", id);
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
