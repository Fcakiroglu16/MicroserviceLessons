using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Logging
{
    public class CorrelationIdMiddleware
    {
        public static  string CorrelationIdHeaderKey = "X-Correlation-ID";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var correlationId = string.Empty;
            if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out var values))
            {
                correlationId = values.First();
           
            }
          

            var logger = httpContext.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>();
            using (logger.BeginScope("{@CorrelationId}", correlationId))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}