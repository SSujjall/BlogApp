using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogApp.Application.Helpers.EmailService.Service
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IBackgroundEmailQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundService> _logger;

        public EmailBackgroundService(
            IBackgroundEmailQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<EmailBackgroundService> logger
        )
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var email in _queue.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var success = await emailService.SendEmailAsync(email);

                    if (success)
                    {
                        _logger.LogInformation("Email sent to {Email}", string.Join(",", email.To));
                    }
                    else
                    {
                        _logger.LogError("Email FAILED to {Email}", string.Join(",", email.To));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email worker crashed");
                }
            }
        }
    }
}
