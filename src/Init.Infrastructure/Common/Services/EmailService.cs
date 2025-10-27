using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Init.Application.Common.Interfaces;

namespace Init.Infrastructure.Common.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromAddress;
    private readonly string _fromName;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        
        // Get SMTP settings from configuration
        _smtpServer = configuration["Email:SmtpServer"] ?? "smtp.example.com";
        _smtpPort = int.TryParse(configuration["Email:SmtpPort"], out var port) ? port : 587;
        _smtpUsername = configuration["Email:Username"] ?? "user@example.com";
        _smtpPassword = configuration["Email:Password"] ?? "password";
        _fromAddress = configuration["Email:FromAddress"] ?? "noreply@init.com";
        _fromName = configuration["Email:FromName"] ?? "init app";
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        await SendEmailAsync([to], subject, body, isHtml);
    }

    public async Task SendEmailAsync(List<string> toAddresses, string subject, string body, bool isHtml = false)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_fromAddress, _fromName);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isHtml;

            // Add all recipients
            foreach (var address in toAddresses)
            {
                mailMessage.To.Add(address);
            }

            await client.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email sent successfully to {RecipientCount} recipients with subject: {Subject}", 
                toAddresses.Count, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipients} with subject: {Subject}", 
                string.Join(", ", toAddresses), subject);
            
            // Re-throw the exception if needed or handle it according to requirements
            throw;
        }
    }
}