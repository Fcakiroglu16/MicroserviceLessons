using Microsoft.AspNetCore.Http;

namespace Logging;

public class AddCorrelationIdRequestHandler:DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddCorrelationIdRequestHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(CorrelationIdMiddleware.CorrelationIdHeaderKey,_httpContextAccessor.HttpContext.Request
        .Headers[CorrelationIdMiddleware.CorrelationIdHeaderKey]
        .First());
        
        
        return base.SendAsync(request, cancellationToken);
    }
}