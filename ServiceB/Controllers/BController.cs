using Microsoft.AspNetCore.Mvc;
using ServiceB.Service;

namespace ServiceB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BController : ControllerBase
{
    private readonly ILogger<BController> _logger;
    private readonly CService _cService;

    public BController(ILogger<BController> logger, CService cService)
    {
        _logger = logger;
        _cService = cService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        
        _logger.LogInformation("Get(B Service)  method  worked");
        await _cService.Get();
        

       
        return Ok();
    }
}