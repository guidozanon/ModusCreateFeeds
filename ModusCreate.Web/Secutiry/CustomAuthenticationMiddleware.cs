using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ModusCreate.Core.Services;
using System.Threading.Tasks;

namespace ModusCreate.Web.Secutiry
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            if (context.Request.HttpContext.User.Identity.IsAuthenticated)
            {
                userService.SetCurrentUser(context.Request.HttpContext.User.Identity.Name);

                if (userService.CurrentUser == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class CustomAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomAuthenticationMiddleware>();
        }
    }
}
