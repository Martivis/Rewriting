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

internal class DeleteOrderMailFactory : MailFactoryBase<OrderDetailsModel>
{
    public DeleteOrderMailFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<MailModel> GetMailModelAsync(OrderDetailsModel model)
    {
        var destinationEmail = await GetUserEmailAsync(model.ClientUid);
        return new()
        {
            DestinationEmail = destinationEmail,
            Subject = "Order deleted",
            Text = $"Order was canceled"
        };
    }
}
