using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webserver.IServices;
using webserver.Services;
using web_server.bl;
using web_server.IServices;
using web_server.Middlewares;
using web_server.Services;
using TokenService = webserver.Services.TokenService;

namespace web_server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(EnvironmentName.Development, builder => builder.WithOrigins(this.Configuration["devClientSideUrl"]).AllowAnyHeader().AllowAnyMethod()));
            services.AddCors(options => options.AddPolicy(EnvironmentName.Production, builder => builder.WithOrigins(this.Configuration["prodClientSideUrl"]).AllowAnyHeader().AllowAnyMethod()));
            services.AddMvc();

            services.AddScoped<IGetUserRequestIdentity, UserRequestIdentity>();
            services.AddScoped<ISetUserRequestIdentity, UserRequestIdentity>();
            new ComponentConfiguration().Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<AuthMiddleware>();
            app.UseCors(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            app.UseMvc();
        }
    }
}
