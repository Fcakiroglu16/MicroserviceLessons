using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataProtectionController : ControllerBase
{
    private readonly IDataProtector _protector;

    public DataProtectionController(IDataProtectionProvider dataProtectionProvider)
    {
        _protector = dataProtectionProvider.CreateProtector("MiniApp");
    }

    [HttpGet]
    public IActionResult Get(string name)
    {
        var decryptedName = _protector.Unprotect(name);
        return Ok(decryptedName);
    }
}