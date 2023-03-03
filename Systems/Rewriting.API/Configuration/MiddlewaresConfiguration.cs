using Rewriting.API.Middlewares;

namespace Rewriting.API.Configuration;

public static class MiddlewaresConfiguration
{
    public static void UseAppMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }
}
