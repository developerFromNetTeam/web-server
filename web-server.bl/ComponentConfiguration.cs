using Microsoft.Extensions.DependencyInjection;
using web_server.common;
using web_server.ibl;

namespace web_server.bl
{
    public class ComponentConfiguration : IComponentConfiguration
    {
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITokenService, TokenService>();

            new web_server.dal.ComponentConfiguration().Register(services);
        }
    }
}
