using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace JwtGenerator;

public class JwtSecurityService(IOptions<GreenTinOptions> options)
{
    private readonly GreenTinOptions _greenTinOptions = options.Value;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public string CreateToken(string userId)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_greenTinOptions.PrivateKey));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("uid", userId)]),
            Expires = DateTime.UtcNow.AddMinutes(_greenTinOptions.ExpiryInMinutes),
            Audience = _greenTinOptions.Audience,
            Issuer = _greenTinOptions.Issuer,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_greenTinOptions.PublicKey)),
            ValidateIssuer = true,
            ValidIssuer = _greenTinOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _greenTinOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return _tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
