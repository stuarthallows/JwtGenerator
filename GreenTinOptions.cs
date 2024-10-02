namespace JwtGenerator;

public class GreenTinOptions
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public int ExpiryInMinutes { get; init; }
    public string PublicKey { get; init; } = string.Empty;
    public string PrivateKey { get; init; } = string.Empty;
    public string KeyId { get; init; } = string.Empty;
}
