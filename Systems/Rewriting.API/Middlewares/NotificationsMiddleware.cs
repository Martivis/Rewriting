using Rewriting.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.API.Middlewares;

public class NotificationsMiddleware
{
    private readonly RequestDelegate _next;

    public NotificationsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, INotificationService notificationService)
    {
        notificationService.SubscribeToOrderEvents();
        await _next.Invoke(context);
    }
}
