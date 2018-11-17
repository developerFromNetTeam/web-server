using Microsoft.Extensions.Logging;
using webserver.IServices;
using web_server.IServices;

namespace web_server.Services
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private ILogger<T> logger;
        private IEventIdGenerator eventIdGenerator;
        public CustomLogger(ILogger<T> logger, IEventIdGenerator eventIdGenerator)
        {
            this.logger = logger;
            this.eventIdGenerator = eventIdGenerator;
        }

        public void LogInformation(string message)
        {
            this.logger.LogInformation($"[{this.eventIdGenerator.GenerateEventId()}] {message}");
        }

        public void LogError(string message)
        {
            this.logger.LogError($"[{this.eventIdGenerator.GenerateEventId()}] {message}");
        }
    }
}
