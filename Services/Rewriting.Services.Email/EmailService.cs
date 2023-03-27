using Rewriting.Services.RabbitMQ;

namespace Rewriting.Services.EMailService;

internal class EmailService : IEmailService
{
    private readonly IRabbitMQ _rabbitMQ;

    public EmailService(IRabbitMQ rabbitMQ)
    {
        _rabbitMQ = rabbitMQ;
    }

    public async Task SendMail(MailModel model)
    {
        await _rabbitMQ.PushAsync(QueueNames.EmailQueue, model);
    }
}