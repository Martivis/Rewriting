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

internal class AcceptResultContractorMailFactory : MailFactoryBase<ContractDetailsModel>
{
    public AcceptResultContractorMailFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<MailModel> GetMailModelAsync(ContractDetailsModel model)
    {
        var destinationEmail = await GetUserEmailAsync(model.ContractorUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "Result accepted",
            Text = $"Contract {model.Uid} has been finished"
        };
    }
}
