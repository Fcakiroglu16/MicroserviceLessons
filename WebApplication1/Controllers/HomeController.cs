using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDataProtector _protector;
    private readonly IKeyManager _keyManager;

    public HomeController(ILogger<HomeController> logger, IDataProtectionProvider dataProtectionProvider,
        IKeyManager keyManager)
    {
        _logger = logger;
        _keyManager = keyManager;
        _protector = dataProtectionProvider.CreateProtector("WebApplication1.Controllers");
    }

    public IActionResult Index()
    {
        TempData["data"] = _protector.Protect("asp.net");

        TempData["TimeToData"] = _protector.ToTimeLimitedDataProtector().Protect("asp.net core", TimeSpan.FromHours(1));

        return View();
    }

    public IActionResult Privacy()
    {
        var data = _protector.Unprotect(TempData["data"].ToString());

        return View();
    }

    public IActionResult KeyManager()
    {
        var allKeys = _keyManager.GetAllKeys();
        Console.WriteLine($"The key ring contains {allKeys.Count} key(s).");
        foreach (var key in allKeys)
            Console.WriteLine($"Key {key.KeyId:B}: Created = {key.CreationDate:u}, IsRevoked = {key.IsRevoked}");

        // revoke all keys in the key ring
        _keyManager.RevokeAllKeys(DateTimeOffset.Now, "Revocation reason here.");
        Console.WriteLine("Revoked all existing keys.");

        // add a new key to the key ring with immediate activation and a 1-month expiration
        _keyManager.CreateNewKey(
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddMonths(1));
        Console.WriteLine("Added a new key.");

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}