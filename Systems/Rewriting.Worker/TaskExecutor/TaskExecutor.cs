using Rewriting.Services.RabbitMQ;
using Rewriting.Services.SmtpSender;

namespace Rewriting.Worker;

public class TaskExecutor : ITaskExecutor
{
    private readonly ILogger<TaskExecutor> _logger;
    private readonly ISmtpSender _smtpSender;
    private readonly IRabbitMQ _rabbitMQ;
    public TaskExecutor(ILogger<TaskExecutor> logger,  
        ISmtpSender smtpSender,
        IRabbitMQ rabbitMQ)
    {
        _logger = logger;
        _smtpSender = smtpSender;
        _rabbitMQ = rabbitMQ;
    }

    private async Task SendMail(MailModel mailModel)
    {
        try
        {
            await _smtpSender.SendEmail(mailModel);
        }
        catch (Exception ex) 
        {
            _logger.LogError($"An error has occered while sending mail.\n{ex}");
            throw new Exception("Unable to send mail");
        }
    }

    public void Start()
    {
        _rabbitMQ.Subscribe<MailModel>(QueueNames.EmailQueue, SendMail);
    }
}
