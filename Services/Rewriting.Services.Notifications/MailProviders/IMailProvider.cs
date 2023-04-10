using Rewriting.Services.Orders;
using Rewriting.Services.SmtpSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Notifications;

internal interface IMailProvider<TModel>
{
    Task<MailModel> GetMailModelAsync(TModel model);
}
