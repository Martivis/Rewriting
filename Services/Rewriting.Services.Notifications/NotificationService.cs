
using Microsoft.Extensions.Logging;
using Rewriting.Services.Contracts;
using Rewriting.Services.EmailService;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;

namespace Rewriting.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IOrderObservable _orderService;
    private readonly IOfferObservable _offerService;
    private readonly IContractObservable _contractService;

    private readonly IEmailService _emailService;
    private readonly IServiceProvider _provider;

    public NotificationService(
        IOrderObservable orderService,
        IOfferObservable offersService,
        IContractObservable contractService,
        IEmailService emailService,
        IServiceProvider serviceProvider
        )
    {
        _orderService = orderService;
        _offerService = offersService;
        _contractService = contractService;
        _emailService = emailService;
        _provider = serviceProvider;
    }

    public void Subscribe()
    {
        _orderService.OnOrderAdd += NotificationHandler(new AddOrderMailFactory(_provider));
        _orderService.OnOrderCancel += NotificationHandler(new CancelOrderMailFactory(_provider));
        _orderService.OnorderDelete += NotificationHandler(new DeleteOrderMailFactory(_provider));

        _offerService.OnOfferAdd += NotificationHandler(new AddOfferClientMailFactory(_provider));
        _offerService.OnOfferAdd += NotificationHandler(new AddOfferContractorMailFactory(_provider));
        _offerService.OnOfferAccept += NotificationHandler(new OfferAcceptedClientMailFactory(_provider));
        _offerService.OnOfferAccept += NotificationHandler(new OfferAcceptedContractorFactory(_provider));

        _contractService.OnResultAdd += NotificationHandler(new AddResultClientMailFactory(_provider));
        _contractService.OnResultAdd += NotificationHandler(new AddResultContractorMailFactory(_provider));
        _contractService.OnResultAccept += NotificationHandler(new AcceptResultClientMailFactory(_provider));
        _contractService.OnResultAccept += NotificationHandler(new AcceptResultContractorMailFactory(_provider));
        _contractService.OnResultDecline += NotificationHandler(new DeclineResultClientMailFactory(_provider));
        _contractService.OnResultDecline += NotificationHandler(new DeclineResultContractorMailFactory(_provider));
        _contractService.OnContractorDecline += NotificationHandler(new DeclineContractorClientMailFactory(_provider));
        _contractService.OnContractorDecline += NotificationHandler(new DeclineContractorContractorMailFactory(_provider));
    }

    private Action<TModel> NotificationHandler<TModel>(MailFactoryBase<TModel> mailProvider)
    {
        return async (TModel model) =>
        {
            var mailModel = await mailProvider.GetMailModelAsync(model);
            await _emailService.SendMail(mailModel);
        };
    }

}