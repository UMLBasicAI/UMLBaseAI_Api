using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Base.DataBaseAndIdentity.DBContext;
using Entry.Registry;
using Microsoft.IdentityModel.JsonWebTokens;

Console.OutputEncoding = Encoding.UTF8;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { Args = args, EnvironmentName = environment }
);
var service = builder.Services;
var configuration = builder.Configuration;

configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

var appName = configuration["AppSettings:AppName"];
if (string.IsNullOrEmpty(appName))
{
    Console.WriteLine("Could not load 'appsettings.json' or key 'AppSettings:AppName' is missing.");
}
else
{
    Console.WriteLine($"Loaded configuration: AppSettings:AppName = {appName}");
}

var providers = ((IConfigurationRoot)configuration).Providers.ToList();
foreach (var provider in providers)
{
    Console.WriteLine($"🔹 Loaded configuration provider: {provider.GetType().Name}");
}

service.RegisterRequireServices(configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var connect = await context.Database.CanConnectAsync();
    if (!connect)
    {
        throw new Exception("Can not connect to database !!");
    }
    Console.WriteLine("Connected to database !!");
}

// Configure the HTTP request pipeline.
app.UseRouting()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseOpenApi()
    .UseSwaggerUi(options =>
    {
        options.Path = string.Empty;
        options.DefaultModelsExpandDepth = -1;
    });
app.MapControllers();

await app.RunAsync(CancellationToken.None);
