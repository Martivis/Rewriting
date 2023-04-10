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

internal class DeclineResultContractorMailFactory : IMailFactory<ContractDetailsModel>
{
    private readonly IServiceProvider _serviceProvider;

    public DeclineResultContractorMailFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<MailModel> GetMailModelAsync(ContractDetailsModel model)
    {
        using var scope = _serviceProvider.CreateScope();
        var userDataService = scope.ServiceProvider.GetService<IUserDataService>();

        var destinationEmail = await userDataService.GetUserEmailAsync(model.ContractorUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "Result declined",
            Text = $"Result of order {model.Uid} was declined"
        };
    }
}
