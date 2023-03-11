using System.Diagnostics;
using OpenTelemetry.Trace;
using WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        
    })
    .Build();

host.Run();