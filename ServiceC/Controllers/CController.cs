using Microsoft.AspNetCore.Mvc;

namespace ServiceC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CController : ControllerBase
{
    private readonly ILogger<CController> _logger;

    public CController(ILogger<CController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Get(C Service)  method  worked");
        return Ok();
    }
}