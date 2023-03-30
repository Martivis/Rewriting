using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rewriting.Common.Exceptions;
using Rewriting.Context.Entities;
using Rewriting.Services.EmailService;
using Rewriting.Services.Orders;
using Rewriting.Services.SmtpSender;
using System.Data;

namespace Rewriting.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IOrderObservable _orderService;

    private readonly IEmailService _emailService;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderService,
        IEmailService emailService,
        IServiceProvider serviceProvider
        )
    {
        _orderService = orderService;
        _emailService = emailService;
        _provider = serviceProvider;
    }

    public void SubscribeToOrderEvents()
    {
        _orderService.OnOrderAdd += AddOrderNotify;
        _orderService.OnOrderCancel += CancelOrderNotify;
        _orderService.OnorderDelete += DeleteOrderNotify;
    }

    private async void AddOrderNotify(OrderDetailsModel model)
    {
        var ownerEmail = await GetUserEmailAsync(model.ClientUid);
        
        MailModel mailModel = new()
        {
            DestinationEmail = ownerEmail,
            Subject = "New order created",
            Text = $"Order {model.Uid} was succesfully created"
        };

        await _emailService.SendMail(mailModel);
    }

    private async void CancelOrderNotify(OrderDetailsModel model)
    {
        var ownerEmail = await GetUserEmailAsync(model.ClientUid);

        MailModel mailModel = new()
        {
            DestinationEmail = ownerEmail,
            Subject = "Order canceled",
            Text = $"Order {model.Uid} was canceled"
        };

        await _emailService.SendMail(mailModel);
    }

    private async void DeleteOrderNotify(OrderDetailsModel model)
    {
        var ownerEmail = await GetUserEmailAsync(model.ClientUid);

        MailModel mailModel = new()
        {
            DestinationEmail = ownerEmail,
            Subject = "Order deleted",
            Text = $"Order {model.Uid} was deleted"
        };

        await _emailService.SendMail(mailModel);
    }

    private async Task<string> GetUserEmailAsync(Guid userUid)
    {
        using var scope = _provider.CreateScope();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();

        var orderOwner = await _userManager.FindByIdAsync(userUid.ToString())
            ?? throw new ProcessException($"User {userUid} not found");

        return orderOwner.Email!;
    }
}