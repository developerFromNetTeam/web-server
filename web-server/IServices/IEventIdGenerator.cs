using System;

namespace webserver.IServices
{
    public interface IEventIdGenerator
    {
        Guid GenerateEventId();
    }
}
