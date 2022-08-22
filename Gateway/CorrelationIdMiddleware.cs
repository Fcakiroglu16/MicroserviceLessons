namespace Gateway;

public class CorrelationIdMiddleware
{
    public const string CorrelationIdHeaderKey = "X-Correlation-ID";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var correlationId = Guid.NewGuid().ToString();
        httpContext.Request.Headers.Add(CorrelationIdHeaderKey, correlationId);
        await _next.Invoke(httpContext);
    }
}