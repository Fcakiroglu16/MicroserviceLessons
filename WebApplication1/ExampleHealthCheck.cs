using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApplication1;

public class ExampleHealthCheck:IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var isHealthy = false;

     

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("An unhealthy result."));

    }
}