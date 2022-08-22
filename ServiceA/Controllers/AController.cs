using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AController : ControllerBase
{
    private readonly ILogger<AController> _logger;

    public AController(ILogger<AController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Get(A Service)  method  worked");
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync("http://ServiceB.api/api/B");

        if (response.IsSuccessStatusCode) _logger.LogInformation("ServiceB was made request successfully");

    
        return Ok();
    }

    [HttpPost]
    public IActionResult Post()
    {
        _logger.LogInformation("Get(A Service)POST  method  worked");
        return Ok("post");
    }
}