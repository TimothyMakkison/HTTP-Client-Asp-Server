using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Net_Core_Server.Encryption;
using Net_Core_Server.Models;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Net_Core_Server.Controllers;

public static class ProtectedEndpoint
{
    private const string BASE_ROUTE = "api/protected/";

    public static IEndpointRouteBuilder MapProtectedEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{BASE_ROUTE}hello", [Authorize] async (ClaimsPrincipal claims) =>
        {
            var username = claims.FindFirstValue(ClaimTypes.Name);
            return Results.Ok($"Hello {username}");
        });

        builder.MapGet($"{BASE_ROUTE}sha1", [Authorize] async (string message) =>
         {
             var hash = CryptoServices.Hasher(message, SHA1.Create());
             return message is null
                 ? Results.BadRequest("Bad Request")
                 : Results.Ok(hash);
         });

        builder.MapGet($"{BASE_ROUTE}sha256", [Authorize] async (string message) =>
        {
            var hash = CryptoServices.Hasher(message, SHA256.Create());
            return message is null
                ? Results.BadRequest("Bad Request")
                : Results.Ok(hash);
        });

        builder.MapGet($"{BASE_ROUTE}getpublickey", [Authorize] async () =>
        {
            return Results.Ok(CryptoServices.RsaPublicKey);
        });

        builder.MapGet($"{BASE_ROUTE}sign", [Authorize] async (string message) =>
         {
             if (message is null)
             {
                 return Results.BadRequest("Query must contain a value.");
             }
             var plaintextBytes = Encoding.UTF8.GetBytes(message);
             var signedData = CryptoServices.RSA.SignData(plaintextBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

             var hexadecimal = BitConverter.ToString(signedData);
             return Results.Ok(hexadecimal);
         });

        builder.MapGet($"{BASE_ROUTE}addfifty", [Authorize(Roles = Role.Admin)] async (string encryptedInteger, string encryptedSymKey, string encryptedIV) =>
         {
             try
             {
                 var symKey = DecryptHex(encryptedSymKey);
                 var IV = DecryptHex(encryptedIV);

                 var integerByteForm = DecryptHex(encryptedInteger);
                 var integerString = Encoding.Default.GetString(integerByteForm);
                 var integer = Convert.ToInt32(integerString);
                 var returnInt = integer + 50;

                 var returnEncrypted = CryptoServices.AesEncrypt(returnInt.ToString(), symKey, IV);
                 var returnString = BitConverter.ToString(returnEncrypted);
                 return Results.Ok(returnString);
             }
             catch
             {
                 return Results.BadRequest("Bad Request");
             }

             static byte[] DecryptHex(string hex)
             {
                 var byteform = CryptoServices.HexToByte(hex);
                 return CryptoServices.RSA.Decrypt(byteform, true);
             }
         });

        return builder;
    }
}