using Hangfire.Dashboard;

namespace Init.Infrastructure.Common.Hangfire;

// Custom authentication filter for Hangfire dashboard
public class HangfireBasicAuthenticationFilter : IDashboardAuthorizationFilter
{
    private readonly string _username;
    private readonly string _password;

    public HangfireBasicAuthenticationFilter(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Check if Basic authentication header is present
        var header = httpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(header) || !header.StartsWith("Basic "))
        {
            SetAuthenticationChallenge(httpContext);
            return false;
        }

        // Validate credentials
        var encodedCredentials = header.Substring("Basic ".Length).Trim();
        var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
        var parts = decodedCredentials.Split(':', 2);
        
        if (parts.Length != 2 || parts[0] != _username || parts[1] != _password)
        {
            SetAuthenticationChallenge(httpContext);
            return false;
        }

        return true;
    }

    private void SetAuthenticationChallenge(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        httpContext.Response.StatusCode = 401;
    }
}