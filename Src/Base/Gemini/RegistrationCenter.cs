using Base.Common.DependencyInjection;
using Base.Config;
using Base.Gemini.Handler;
using Base.Mail.Handler;
using System.Net;

namespace Base.Gemini;

internal sealed class RegistrationCenter : IExternalServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        AddGeminiService(services, configuration);
        return services;
    }

    internal static void AddGeminiService(IServiceCollection services, IConfiguration configuration)
    {
        var geminiOptions = configuration
            .GetRequiredSection("Gemini")
            .Get<GeminiOption>();

        services
            .AddSingleton<IGeminiService, GeminiService>()
            .MakeSingletonLazy<IGeminiService>();
        services.AddHttpClient<IGeminiService, GeminiService>((provider, client) =>
        {
            client.BaseAddress = new Uri(geminiOptions.Endpoint);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                Proxy = new WebProxy("http://20.84.44.128:3128"),
                UseProxy = true
            };
        });

        services.AddSingleton(geminiOptions);
    }
}
