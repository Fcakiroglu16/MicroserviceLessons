using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataProtectionController : ControllerBase
{
    private readonly IDataProtector _protector;
    private readonly HttpClient _client;

    public DataProtectionController(IDataProtectionProvider dataProtectionProvider, HttpClient client)
    {
        _client = client;
        // _protector = dataProtectionProvider.CreateProtector("MiniApp");
        _protector = dataProtectionProvider.CreateProtector("MiniApp2");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var name = "asp.net core";
        var encryptedName = _protector.Protect(name);
        var response = await _client.GetAsync($"https://localhost:7211/api/DataProtection?name={encryptedName}");
        var responseText = response.Content.ReadAsStringAsync();
        return Ok(responseText);
    }
}