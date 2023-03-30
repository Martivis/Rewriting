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
    private readonly IOrderObservable _orderObservable;

    private readonly IEmailService _emailService;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderObservable,
        IEmailService emailService,
        IServiceProvider serviceProvider
        )
    {
        _orderObservable = orderObservable;
        _emailService = emailService;
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
        var ownerEmail = await GetUserEmailByUid(model.ClientUid);
        
        MailModel mailModel = new()
        {
            DestinationEmail = ownerEmail,
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

    private async Task<string> GetUserEmailByUid(Guid userUid)
    {
        using var scope = _provider.CreateScope();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();

        var orderOwner = await _userManager.FindByIdAsync(userUid.ToString())
            ?? throw new ProcessException($"User {userUid} not found");

        return orderOwner.Email!;
    }
}