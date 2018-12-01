using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyIdeaPool
{
    public static class SecurityHeadersExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersMiddleWare(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
    
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {        
            var accessToken = context.Request.Headers["X-Access-Token"];
            if(!string.IsNullOrEmpty(accessToken))
                context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
           
            await _next(context);
        }
    }
}