namespace Microservice1.API.Services;

public class WeatherForcastService
{
    private readonly HttpClient _client;

    public WeatherForcastService(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> Get()
    {

        var response = await _client.GetAsync("WeatherForecast");

        return await response.Content.ReadAsStringAsync();
    }
}