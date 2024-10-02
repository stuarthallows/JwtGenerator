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

services.AddSingleton<JwtSecurityTokenService>()
        .AddLogging(builder => builder.AddConsole())
        .Configure<GreenTinOptions>(config.GetSection(nameof(GreenTinOptions)));

var provider = services.BuildServiceProvider();

var jwtSecurity = provider.GetRequiredService<JwtSecurityTokenService>();
var logger = provider.GetRequiredService<ILogger<Program>>();

try
{
    var token = jwtSecurity.CreateToken("john.doe@larch.com");
    logger.LogInformation("{Token}", token);
    
    var principal = jwtSecurity.ValidateToken(token);
    logger.LogInformation("Validated {Uid}:", principal.FindFirst(c => c.Type == "uid"));
}
catch (NotSupportedException e)
{
    logger.LogError(e, "Not supported to process token");
}
catch (ArgumentException e)
{
    logger.LogError(e, "Failed to process token");
}
catch (SecurityTokenSignatureKeyNotFoundException e)
{
    logger.LogError(e, "Failed to process token");
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
