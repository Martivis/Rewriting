using Rewriting.Common.Exceptions;
using Rewriting.Common.Helpers;
using Rewriting.Common.Responses;
using System.Text.Json;

namespace Rewriting.API.Middlewares;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ErrorResponse response = null;
        try
        {
            await _next.Invoke(context);
        }
        catch (ProcessException exception)
        {
            response = exception.ToErrorResponse();
        }
        finally
        {
            if (response is not null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                await context.Response.StartAsync();
                await context.Response.CompleteAsync();
            }
        }
    }
}
