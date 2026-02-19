using Tradebox.Services;

namespace Tradebox.Middleware;

/// <summary>
/// 301 Permanent Redirect middleware.
/// Her request'te Custom Table'daki redirect kurallarini kontrol eder.
/// Pipeline'da en basta olmali (UseRouting'den once).
/// </summary>
public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RedirectMiddleware> _logger;

    public RedirectMiddleware(RequestDelegate next, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IRedirectService redirectService)
    {
        var path = context.Request.Path.Value;

        if (!string.IsNullOrWhiteSpace(path))
        {
            var redirectTo = redirectService.GetRedirectUrl(path);

            if (redirectTo != null)
            {
                // Query string'i koru
                var queryString = context.Request.QueryString.Value;
                var fullRedirectUrl = redirectTo + queryString;

                _logger.LogInformation(
                    "301 Redirect: {From} -> {To}",
                    path + queryString, fullRedirectUrl);

                context.Response.StatusCode = 301;
                context.Response.Headers["Location"] = fullRedirectUrl;
                return;
            }
        }

        await _next(context);
    }
}
