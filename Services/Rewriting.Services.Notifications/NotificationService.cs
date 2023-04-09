using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rewriting.Common.Exceptions;
using Rewriting.Context.Entities;
using Rewriting.Services.EmailService;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;
using Rewriting.Services.SmtpSender;
using System.Data;

namespace Rewriting.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IOrderObservable _orderService;
    private readonly IOffersObservable _offersService;

    private readonly IEmailService _emailService;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderService,
        IOffersObservable offersService,
        IEmailService emailService,
        IServiceProvider serviceProvider
        )
    {
        _orderService = orderService;
        _offersService = offersService;
        _emailService = emailService;
        _provider = serviceProvider;
    }

    public void Subscribe()
    {
        _orderService.OnOrderAdd += AddOrderNotify;
        _orderService.OnOrderCancel += CancelOrderNotify;
        _orderService.OnorderDelete += DeleteOrderNotify;

        _offersService.OnOfferAdd += AddOfferNotifyClient;
        _offersService.OnOfferAdd += AddOfferNotifyContractor;
        _offersService.OnOfferAccept += AcceptOfferNotifyClient;
        _offersService.OnOfferAccept += AcceptOfferNotifyContractor;
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

    private async void AddOfferNotifyClient(OfferModel model)
    {
        var clientEmail = await GetUserEmailAsync(model.ClientUid);

        MailModel mailModel = new()
        {
            DestinationEmail = clientEmail,
            Subject = "New offer",
            Text = $"Offer {model.Uid} was added to your order"
        };

        await _emailService.SendMail(mailModel);
    }

    private async void AddOfferNotifyContractor(OfferModel model)
    {
        var contractorEmail = await GetUserEmailAsync(model.ContractorUid);

        MailModel mailModel = new()
        {
            DestinationEmail = contractorEmail,
            Subject = "Offer added",
            Text = $"Offer {model.Uid} was successfully created"
        };

        await _emailService.SendMail(mailModel);
    }

    private async void AcceptOfferNotifyClient(OfferModel model)
    {
        var clientEmail = await GetUserEmailAsync(model.ClientUid);

        MailModel mailModel = new()
        {
            DestinationEmail = clientEmail,
            Subject = "Offer accepted",
            Text = $"Offer {model.Uid} was successfully accepted"
        };

        await _emailService.SendMail(mailModel);
    }

    private async void AcceptOfferNotifyContractor(OfferModel model)
    {
        var contractorEmail = await GetUserEmailAsync(model.ContractorUid);

        MailModel mailModel = new()
        {
            DestinationEmail = contractorEmail,
            Subject = "New offer",
            Text = $"Your offer {model.Uid} was accepted"
        };

        await _emailService.SendMail(mailModel);
    }

    private async Task<string> GetUserEmailAsync(Guid userUid)
    {
        using var scope = _provider.CreateScope();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var orderOwner = await _userManager.FindByIdAsync(userUid.ToString())
            ?? throw new ProcessException($"User {userUid} not found");

        return orderOwner.Email!;
    }

}