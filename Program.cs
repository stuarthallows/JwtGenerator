using Microsoft.Extensions.DependencyInjection;

using JwtGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.AddSingleton<JwtSecurityService>()
        .AddLogging(builder => builder.AddConsole())
        .Configure<GreenTinOptions>(config.GetSection(nameof(GreenTinOptions)));

var provider = services.BuildServiceProvider();

var jwtSecurity = provider.GetRequiredService<JwtSecurityService>();
var logger = provider.GetRequiredService<ILogger<Program>>();

try
{
    var token = jwtSecurity.CreateToken("john.doe@larch.com");
    logger.LogInformation("Generated token: {Token}", token);

    var claims = jwtSecurity.ValidateToken(token);
    foreach (var claim in claims.Claims)
    {
        logger.LogInformation("{Type}: {Value}", claim.Type, claim.Value);
    }
}
catch (SecurityTokenSignatureKeyNotFoundException e)
{
    logger.LogError(e, "Failed to process token");
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
