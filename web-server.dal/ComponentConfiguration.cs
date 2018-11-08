using Microsoft.Extensions.DependencyInjection;
using web_server.common;
using web_server.idal;

namespace web_server.dal
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IMongoContext, MongoContext>();
        }
    }
}
