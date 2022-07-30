// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

using var rsa = RSA.Create();
Console.WriteLine(
    $"-----Private key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPrivateKey())}{Environment.NewLine}");
Console.WriteLine($"-----Public key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPublicKey())}");