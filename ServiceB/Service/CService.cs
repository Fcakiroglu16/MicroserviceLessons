using ServiceB.Controllers;

namespace ServiceB.Service;

public class CService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BController> _logger;
    public CService(HttpClient httpClient, ILogger<BController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task Get()
    {
        var response = await _httpClient.GetAsync("c");
        if (response.IsSuccessStatusCode) _logger.LogInformation("ServiceC was made request successfully");
        
        
        
    }
}