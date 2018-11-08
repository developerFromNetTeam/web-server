using System;
using Microsoft.Extensions.DependencyInjection;
using web_server.common;

namespace web_server.ioc
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        public void Register(IServiceCollection services)
        {
            new web_server.bl.ComponentConfiguration().Register(services);
            new web_server.dal.ComponentConfiguration().Register(services);
        }
    }
}
