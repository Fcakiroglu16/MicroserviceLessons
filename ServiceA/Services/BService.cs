using ServiceA.Controllers;

namespace ServiceA.Services;

public class BService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AController> _logger;
    public BService(HttpClient httpClient, ILogger<AController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task Get()
    {
        var response = await _httpClient.GetAsync("b");
        if (response.IsSuccessStatusCode) _logger.LogInformation("ServiceB was made request successfully");
        
        
        
    }
}