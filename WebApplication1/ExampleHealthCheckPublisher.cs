using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApplication1;

public class ExampleHealthCheckPublisher:IHealthCheckPublisher
{
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        Console.WriteLine(report.Status == HealthStatus.Healthy ? "Status :healthy" : "Status :Unhealthy");

        return Task.CompletedTask;
    }
}