using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webserver.IServices;
using webserver.Services;
using web_server.bl;
using web_server.IServices;
using web_server.Middlewares;
using web_server.Services;

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

            var ui = new UserRequestIdentity();
            services.AddScoped<IGetUserRequestIdentity>(provider => ui);
            services.AddScoped<ISetUserRequestIdentity>(provider => ui);

            
            services.AddScoped(typeof(ICustomLogger<>), typeof(CustomLogger<>));
            services.AddScoped<IEventIdGenerator, EventIdGenerator>();

            new ComponentConfiguration().Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Development: {env.IsDevelopment()}, Production: {env.IsProduction()}");
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production");
            }
            app.UseMiddleware<AuthMiddleware>();
            app.UseMvc();
        }
    }
}
