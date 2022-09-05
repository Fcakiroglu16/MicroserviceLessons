using Microservice1.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollyController : ControllerBase
{

    private readonly WeatherForcastService _weatherForcastService;

    public PollyController(WeatherForcastService weatherForcastService)
    {
        _weatherForcastService = weatherForcastService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {

    
        return Ok(await _weatherForcastService.Get());
    }
}