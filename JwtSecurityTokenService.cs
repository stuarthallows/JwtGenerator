using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JwtGenerator;

public class JwtSecurityTokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public string CreateToken(string userId, string audience, string issuer, string privateKey, int expiryInMinutes)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("uid", userId)]),
            Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new JsonWebKey(privateKey), SecurityAlgorithms.EcdsaSha256)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateToken(string token, string audience, string issuer, string publicKey)
    {
        var validationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new JsonWebKey(publicKey),
            ValidateLifetime = true
        };

        return _tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
