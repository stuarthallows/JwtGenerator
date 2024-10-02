namespace JwtGenerator;

public class GreenTinOptions
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public int ExpiryInMinutes { get; init; }
    public string PrivateKey { get; init; } = string.Empty;
}
