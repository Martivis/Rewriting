using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.EMailService;

public interface IEmailService
{
    Task SendMail(MailModel model);
}
