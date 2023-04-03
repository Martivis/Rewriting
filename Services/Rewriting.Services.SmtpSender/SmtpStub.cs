using Microsoft.Extensions.Logging;

namespace Rewriting.Services.SmtpSender;

public class SmtpStub : ISmtpSender
{
    private readonly ILogger<SmtpStub> _logger;

    public SmtpStub(ILogger<SmtpStub> logger)
    {
        _logger = logger;
    }

    public async Task SendEmail(MailModel mailModel)
    {
        _logger.LogInformation("Stub::: Mail sent:\n" +
            $"To: {mailModel.DestinationEmail}\n" +
            $"Subject: {mailModel.Subject}\n" +
            $"Body: {mailModel.Text}");
    }
}
