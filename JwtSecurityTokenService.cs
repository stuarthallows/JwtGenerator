using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtGenerator;

public class JwtSecurityTokenService(IOptions<GreenTinOptions> options)
{
    private readonly GreenTinOptions _options = options.Value;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public string CreateToken(string userId)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("uid", userId)]),
            Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryInMinutes),
            Audience = _options.Audience,
            Issuer = _options.Issuer,
            SigningCredentials = new SigningCredentials(new JsonWebKey(_options.PrivateKey), SecurityAlgorithms.EcdsaSha256)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new JsonWebKey(_options.PublicKey),
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return _tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
