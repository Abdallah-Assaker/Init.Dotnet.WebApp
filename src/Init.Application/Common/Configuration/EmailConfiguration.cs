namespace Init.Application.Common.Configuration;

/// <summary>
/// Configuration class for email settings
/// </summary>
public class EmailConfiguration
{
    /// <summary>
    /// List of recipients for performance report emails
    /// </summary>
    public List<string> PerformanceReportRecipients { get; set; } = new();
}
