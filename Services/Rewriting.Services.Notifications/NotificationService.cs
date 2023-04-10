using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rewriting.Common.Exceptions;
using Rewriting.Context.Entities;
using Rewriting.Services.EmailService;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;
using Rewriting.Services.SmtpSender;
using Rewriting.Services.UserAccount;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace Rewriting.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IOrderObservable _orderService;
    private readonly IOfferObservable _offersService;

    private readonly IEmailService _emailService;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderService,
        IOfferObservable offersService,
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
        _orderService.OnOrderAdd += NotificationHandler(new AddOrderMailFactory(_provider));
        _orderService.OnOrderCancel += NotificationHandler(new CancelOrderMailFactory(_provider));
        _orderService.OnorderDelete += NotificationHandler(new DeleteOrderMailFactory(_provider));

        _offersService.OnOfferAdd += NotificationHandler(new AddOfferClientMailFactory(_provider));
        _offersService.OnOfferAdd += NotificationHandler(new AddOfferContractorMailFactory(_provider));
        _offersService.OnOfferAccept += NotificationHandler(new OfferAcceptedClientMailFactory(_provider));
        _offersService.OnOfferAccept += NotificationHandler(new OfferAcceptedContractorFactory(_provider));
    }

    private Action<TModel> NotificationHandler<TModel>(IMailProvider<TModel> mailProvider)
    {
        return async (TModel model) =>
        {
            var mailModel = await mailProvider.GetMailModelAsync(model);
            await _emailService.SendMail(mailModel);
        };
    }

}