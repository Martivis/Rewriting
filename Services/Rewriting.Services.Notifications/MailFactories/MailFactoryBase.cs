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

internal abstract class MailFactoryBase<TModel>
{
    private readonly IServiceProvider _serviceProvider;

    protected MailFactoryBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public abstract Task<MailModel> GetMailModelAsync(TModel model);

    protected async Task<string> GetUserEmailAsync(Guid Uid)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var userDataService = scope.ServiceProvider.GetService<IUserDataService>();

        var email = await userDataService.GetUserEmailAsync(Uid);
        return email;
    }
}
