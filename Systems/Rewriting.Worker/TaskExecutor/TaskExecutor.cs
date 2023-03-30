using Rewriting.Services.RabbitMQ;
using Rewriting.Services.SmtpSender;

namespace Rewriting.Worker;

public class TaskExecutor : ITaskExecutor
{
    private readonly ILogger<TaskExecutor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitMQ _rabbitMQ;
    public TaskExecutor(ILogger<TaskExecutor> logger, IServiceProvider serviceProvider, IRabbitMQ rabbitMQ)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _rabbitMQ = rabbitMQ;
    }

    private async Task SendMail(MailModel mailModel)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var smtpSender = scope.ServiceProvider.GetService<ISmtpSender>();

            if (smtpSender == null)
            {
                _logger.LogCritical($"Unable to resolve ISmtpSender");
            }
            else
            {
                await smtpSender.SendEmail(mailModel);
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError($"An error has occered while sending mail.\n{ex}");
        }
    }

    public void Start()
    {
        _rabbitMQ.Subscribe<MailModel>(QueueNames.EmailQueue, SendMail);
    }
}
