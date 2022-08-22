using Microsoft.AspNetCore.Mvc;
using ServiceA.Services;

namespace ServiceA.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AController : ControllerBase
{
    private readonly ILogger<AController> _logger;
    private readonly BService _bService;
    public AController(ILogger<AController> logger, BService bService)
    {
        _logger = logger;
        _bService = bService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Get(A Service)  method  worked");
        await _bService.Get();
        return Ok();
    }

    [HttpPost]
    public IActionResult Post()
    {
        _logger.LogInformation("Get(A Service)POST  method  worked");
        return Ok("post");
    }
}