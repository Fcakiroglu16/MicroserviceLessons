using System.Diagnostics;

namespace WorkerService;

public class Worker : BackgroundService
{
    private static readonly HttpClient Client = new();
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var activity = AppActivity.Source.StartActivity("Microservice API_Post", ActivityKind.Client))
            {
                activity!.AddBaggage("userId", "123456"); // added to header
                activity.AddTag("tag1", "custom tag content");
                var response = await Client.GetAsync("http://localhost:5181/WeatherForecast", stoppingToken);


                _logger.LogInformation(response.IsSuccessStatusCode
                    ? "Microservice1 request has completed successfully"
                    : "Microservice1 request has not completed");
            }


            await Task.Delay(1000000, stoppingToken);
        }
    }
}