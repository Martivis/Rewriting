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

internal class OfferAcceptedContractorFactory : MailFactoryBase<OfferModel>
{
    public OfferAcceptedContractorFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<MailModel> GetMailModelAsync(OfferModel model)
    {
        var destinationEmail = await GetUserEmailAsync(model.ContractorUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "Offer accepted",
            Text = $"Your offer {model.Uid} was accepted"
        };
    }
}
