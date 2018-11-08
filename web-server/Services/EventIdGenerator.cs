using System;
using webserver.IServices;

namespace webserver.Services
{
    public class EventIdGenerator : IEventIdGenerator
    {
        private Guid requestEventId;

        public EventIdGenerator()
        {
            this.requestEventId = Guid.NewGuid();
        }
        public Guid GenerateEventId()
        {
            return this.requestEventId;
        }
    }
}
