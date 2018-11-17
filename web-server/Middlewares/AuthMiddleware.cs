using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using web_server.ibl;
using web_server.IServices;

namespace web_server.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISetUserRequestIdentity setUserRequestIdentity, IAuthService authService, ICustomLogger<AuthMiddleware> customLogger)
        {
            customLogger.LogInformation($"HTTP Request: {context.Request.Path.Value.ToLower()}");

            if (context.Request.Path.Value.ToLower() != "/api/auth/login")
            {
                var token = context.Request.Headers["auth-token"].ToString();

                if (string.IsNullOrWhiteSpace(token))
                {
                    customLogger.LogInformation("Token is missing.");
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Token is missing.");
                }
                else
                {
                    var userInfo = await authService.GetUserInfoByAuthToken(token);

                    if (userInfo == null)
                    {
                        customLogger.LogInformation("Token is invalid.");
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Token is invalid.");
                    }
                    else
                    {
                        customLogger.LogInformation("Token is valid.");
                        setUserRequestIdentity.SetUser(userInfo);
                        await _next.Invoke(context);
                    }
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
