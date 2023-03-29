using Microsoft.Extensions.Logging;

namespace Rewriting.Services.SmtpSender;

public class SmtpStub : ISmtpSender
{
    private readonly ILogger _logger;

    public void SendEmail(MailModel mailModel)
    {
        _logger.LogDebug("Stub::: Mail sent:\n" +
            $"From: {mailModel.SourceName} {mailModel.SourceEmail}\n" +
            $"To: {mailModel.DestinationEmail}\n" +
            $"Subject: {mailModel.Subject}\n" +
            $"Body: {mailModel.Text}");
    }
}
