using Rewriting.Services.SmtpSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

internal static class ResetPasswordMailFactory
{
    public static MailModel GetMailModel(string targetEmail, string token)
    {
        return new MailModel
        {
            DestinationEmail = targetEmail,
            Subject = "Reset password",
            Text = $"Your token for password reset:\n{token}"
        };
    }
}
