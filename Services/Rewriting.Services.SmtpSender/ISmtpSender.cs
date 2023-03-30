using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.SmtpSender;

public interface ISmtpSender
{
    public Task SendEmail(MailModel mailModel);
}
