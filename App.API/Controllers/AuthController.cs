using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly RsaServices _rsaServices;

    public AuthController(RsaServices rsaServices)
    {
        _rsaServices = rsaServices;
    }

    [HttpPost]
    public IActionResult GenerateTokenAsymmetric()
    {
        var jwtDate = DateTime.Now;

        var jwt = new JwtSecurityToken(
            audience: "jwt-audience",
            issuer: "jwt-issuer",
            claims: new List<Claim>() { new (ClaimTypes.Name, "exampleUserName") },
            notBefore: jwtDate,
            expires: jwtDate.AddSeconds(10),
            signingCredentials: _rsaServices.GetSigningCredentialsWithPrivateKey()
        );

        return Ok(new { jwt = new JwtSecurityTokenHandler().WriteToken(jwt) });
    }

    [HttpGet]
    [Authorize]
    public IActionResult ValidateTokenAsymmetric()
    {
        return Ok(User.Identity!.Name);
    }
}