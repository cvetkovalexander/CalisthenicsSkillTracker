using Microsoft.AspNetCore.Http;

namespace CalisthenicsSkillTracker.Infrastructure.Middlewares
{
    public class AdminRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRedirectMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                bool isAdmin = context.User.IsInRole("Admin");
                string path = context.Request.Path.Value ?? string.Empty;

                if (isAdmin && path == "/" && HttpMethods.IsGet(context.Request.Method))
                {
                    context.Response.Redirect("/Admin/Home/Index");
                    return;
                }
            }

            await this._next(context);
        }
    }
}
