using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace WorkerService;

public class AppActivity
{
    public static ActivitySource Source = new ActivitySource("WorkerService.Console.App", "1.0.0");

    static AppActivity()
    {
         Sdk.CreateTracerProviderBuilder().AddSource("WorkerService.Console.App")
            .AddHttpClientInstrumentation().AddConsoleExporter().Build();
    }


    
    
}
