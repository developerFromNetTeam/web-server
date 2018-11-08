using Microsoft.Extensions.DependencyInjection;

namespace web_server.common
{
    public interface IComponentConfiguration
    {
        void Register(IServiceCollection services);
    }
}
