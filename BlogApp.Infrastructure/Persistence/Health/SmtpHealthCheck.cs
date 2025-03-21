using BlogApp.Application.Helpers.EmailService.Config;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlogApp.Infrastructure.Persistence.Health
{
    public class SmtpHealthCheck : IHealthCheck
    {
        private readonly EmailConfig _emailConfig;

        public SmtpHealthCheck(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password, cancellationToken);

                return HealthCheckResult.Healthy("SMTP server is reachable.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SMTP server not reachable.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
                client.Dispose();
            }
        }
    }
}
