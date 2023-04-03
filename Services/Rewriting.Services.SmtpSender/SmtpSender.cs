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

    public async Task SendEmail(MailModel mailModel)
    {
        var message = CreateMessage(mailModel);

        using var client = new SmtpClient();

        client.Connect(_settings.Host, _settings.Port, _settings.UseSSL);
        client.Authenticate(_settings.UserName, _settings.Password);
        client.Send(message);
    }

    private MimeMessage CreateMessage(MailModel mailModel)
    {
        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("", mailModel.DestinationEmail));
        message.Subject = mailModel.Subject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = mailModel.Text
        };

        return message;
    }
}