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

internal class AddOfferClientMailFactory : MailFactoryBase<OfferModel>
{
    public AddOfferClientMailFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<MailModel> GetMailModelAsync(OfferModel model)
    {
        var destinationEmail = await GetUserEmailAsync(model.ClientUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "New offer",
            Text = $"Offer {model.Uid} was added to your order"
        }; 
    }
}
