using Microsoft.Extensions.DependencyInjection;

using JwtGenerator;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.AddSingleton<JwtSecurityService>()
        .Configure<GreenTinOptions>(config.GetSection(nameof(GreenTinOptions)));

var provider = services.BuildServiceProvider();

var jwtSecurity = provider.GetRequiredService<JwtSecurityService>();

var token = jwtSecurity.CreateToken("john.doe@larch.com");
Console.WriteLine(token);

var claims = jwtSecurity.ValidateToken(token);
foreach (var claim in claims.Claims)
{
    Console.WriteLine($"{claim.Type}: {claim.Value}");
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
