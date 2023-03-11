using Microsoft.AspNetCore.Mvc;

namespace Microservice1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public WeatherForecastController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("httpClientWithMicroservice2");
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var response = await _httpClient.GetAsync("/WeatherForecast");

        var context = await response.Content.ReadAsStringAsync();

        return Ok(context);
    }
}