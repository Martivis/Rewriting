using Microsoft.Extensions.DependencyInjection;
using Rewriting.Services.Contracts;
using Rewriting.Services.SmtpSender;
using Rewriting.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Notifications;

internal class DeclineResultClientMailFactory : MailFactoryBase<ContractDetailsModel>
{
    public DeclineResultClientMailFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<MailModel> GetMailModelAsync(ContractDetailsModel model)
    {
        var destinationEmail = await GetUserEmailAsync(model.ClientUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "Result declined",
            Text = $"Result of order {model.Uid} was succesfully declined"
        };
    }
}
