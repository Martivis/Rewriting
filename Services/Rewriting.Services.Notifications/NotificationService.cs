using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rewriting.Common.Exceptions;
using Rewriting.Context.Entities;
using Rewriting.Services.EmailService;
using Rewriting.Services.Orders;
using System.Data;

namespace Rewriting.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IOrderObservable _orderObservable;

    private readonly IEmailService _emailService;
    //private readonly UserManager<UserIdentity> _userManager;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderObservable,
        IEmailService emailService,
        //UserManager<UserIdentity> userManager
        IServiceProvider serviceProvider
        )
    {
        _orderObservable = orderObservable;
        _emailService = emailService;
        //_userManager = userManager;
        _provider = serviceProvider;
    }

    public void SubscribeToOrderEvents()
    {
        _orderObservable.OnOrderAdd += AddOrderNotify;
        _orderObservable.OnOrderCancel += CancelOrderNotify;
        _orderObservable.OnorderDelete += DeleteOrderNotify;
    }

    private async void AddOrderNotify(OrderDetailsModel model)
    {
        using var scope = _provider.CreateScope();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();

        var orderOwner = await _userManager.FindByIdAsync(model.ClientUid.ToString())
            ?? throw new ProcessException($"User {model.ClientUid} not found");

        MailModel mailModel = new()
        {
            DestinationEmail = orderOwner.Email!,
            Subject = "New order created",
            Text = $"Order {model.Uid} was succesfully created"
        };

        await _emailService.SendMail(mailModel);
    }

    private void CancelOrderNotify(Guid orderUid)
    {

    }

    private void DeleteOrderNotify(Guid orderUid)
    {

    }
}