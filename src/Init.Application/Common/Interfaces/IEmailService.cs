namespace Init.Application.Common.Interfaces;

/// <summary>
/// Interface for sending emails
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <param name="isHtml">Whether the body content is HTML</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    
    /// <summary>
    /// Sends an email to multiple recipients
    /// </summary>
    /// <param name="toAddresses">List of recipient email addresses</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <param name="isHtml">Whether the body content is HTML</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendEmailAsync(List<string> toAddresses, string subject, string body, bool isHtml = false);
}