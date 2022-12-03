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

    [HttpPost]
    public async Task<IActionResult> Post(RequestModel requestModel)
    {
        
        _logger.LogInformation("Get(B Service)  method  worked");




        return Ok(requestModel);
    }
}

public class RequestModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}