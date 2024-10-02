namespace JwtGenerator.Configuration;

public class JwtOptions
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public int ExpiryInMinutes { get; init; }
    
    /// <summary>
    /// The public JSON Web Key (JWK) used to validate the token. 
    /// </summary>
    public string PublicKey { get; init; } = string.Empty;
    
    /// <summary>
    /// The private JSON Web Key (JWK) used to sign the token. 
    /// </summary>
    public string PrivateKey { get; init; } = string.Empty;
}
