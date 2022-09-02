using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;

namespace Microservice1.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    readonly DiscoveryHttpClientHandler _discoveryHttpClientHandler;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IDiscoveryClient discoveryClient)
    {
        _logger = logger;
        _discoveryHttpClientHandler = new(discoveryClient);
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpPost]
    public async  Task<IActionResult> Post()
    {

        using HttpClient httpClient = new(_discoveryHttpClientHandler, false);
        var response= await httpClient.PostAsync("http://microserviceB/WeatherForecast", null);

        var content = await response.Content.ReadAsStringAsync();
        return Ok(new { Result= "Request has made to Microservice 2", Response= content });
    }

    [HttpPut]
    public async Task<IActionResult> Put()
    {

        using HttpClient httpClient = new(_discoveryHttpClientHandler, false);
        var response = await httpClient.GetAsync("http://microserviceB/WeatherForecast");

        var content = await response.Content.ReadAsStringAsync();
        return Ok(new { Result = "Request has made to Microservice 2", Response = content });
    }




}