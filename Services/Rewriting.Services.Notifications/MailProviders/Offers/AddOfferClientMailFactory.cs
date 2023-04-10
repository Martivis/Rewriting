using Microsoft.Extensions.DependencyInjection;
using Rewriting.Services.Offers;
using Rewriting.Services.SmtpSender;
using Rewriting.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Notifications;

internal class AddOfferClientMailFactory : IMailProvider<OfferModel>
{
    private readonly IServiceProvider _serviceProvider;

    public AddOfferClientMailFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<MailModel> GetMailModelAsync(OfferModel model)
    {
        using var scope = _serviceProvider.CreateScope();
        var userDataService = scope.ServiceProvider.GetService<IUserDataService>();

        var destinationEmail = await userDataService.GetUserEmailAsync(model.ClientUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "New offer",
            Text = $"Offer {model.Uid} was added to your order"
        }; 
    }
}
