using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BController : ControllerBase
{
    private readonly ILogger<BController> _logger;

    public BController(ILogger<BController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        
        _logger.LogInformation("Get(B Service)  method  worked");
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync("http://ServiceC.api/api/C");

        if (response.IsSuccessStatusCode) _logger.LogInformation("ServiceC was made request successfully");

       
        return Ok();
    }
}