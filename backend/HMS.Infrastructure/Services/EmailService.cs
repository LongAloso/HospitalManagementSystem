using HMS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HMS.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string body)
    {
        _logger.LogInformation($"[MOCK EMAIL] Đang gửi tới: {toEmail}");
        _logger.LogInformation($"[MOCK EMAIL] Tiêu đề: {subject}");
        _logger.LogInformation($"[MOCK EMAIL] Nội dung: {body}");

        return Task.CompletedTask;
    }
}