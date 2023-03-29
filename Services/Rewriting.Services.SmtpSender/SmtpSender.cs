using MailKit.Net.Smtp;
using MimeKit;

namespace Rewriting.Services.SmtpSender;

internal class SmtpSender : ISmtpSender
{
    private readonly SmtpSettings _settings;

    public SmtpSender(SmtpSettings mailSettings)
    {
        _settings = mailSettings;
    }

    public void SendEmail(MailModel mailModel)
    {
        var message = CreateMessage(mailModel);

        using var client = new SmtpClient();

        client.Connect(_settings.Uri);
        client.Authenticate(_settings.UserName, _settings.Password);
        client.Send(message);
    }

    private static MimeMessage CreateMessage(MailModel mailModel)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(mailModel.SourceName, mailModel.SourceEmail));
        message.To.Add(new MailboxAddress("", mailModel.DestinationEmail));
        message.Subject = mailModel.Subject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = mailModel.Text
        };

        return message;
    }
}