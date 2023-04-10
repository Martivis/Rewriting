using Microsoft.Extensions.DependencyInjection;
using Rewriting.Services.Orders;
using Rewriting.Services.SmtpSender;
using Rewriting.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Notifications;

internal class AddOrderMailFactory : IMailFactory<OrderDetailsModel>
{
    private readonly IServiceProvider _serviceProvider;

    public AddOrderMailFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<MailModel> GetMailModelAsync(OrderDetailsModel model)
    {
        using var scope = _serviceProvider.CreateScope();
        var userDataService = _serviceProvider.GetService<IUserDataService>();

        var destinationEmail = await userDataService.GetUserEmailAsync(model.ClientUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "New order created",
            Text = $"Order was succesfully created"
        };
    }
}
