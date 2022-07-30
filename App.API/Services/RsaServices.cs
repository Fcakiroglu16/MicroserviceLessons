using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace App.API.Services;

public class RsaServices
{
    private readonly IConfiguration _configuration;
    private readonly RSA _rsa;

    public RsaServices(IConfiguration configuration)
    {
        _configuration = configuration;
        _rsa = RSA.Create();
    }

    public SigningCredentials GetSigningCredentialsWithPrivateKey()
    {
        _rsa.ImportRSAPrivateKey(
            Convert.FromBase64String(_configuration["Jwt:Asymmetric:PrivateKey"]),
            out _);

        return new SigningCredentials(
            new RsaSecurityKey(_rsa),
            SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
        );
    }

    public SecurityKey GetSecurityKeyWithPublicKey()
    {
        _rsa.ImportRSAPublicKey(
            Convert.FromBase64String(_configuration["Jwt:Asymmetric:PublicKey"]),
            out _
        );
        return new RsaSecurityKey(_rsa);
    }
}