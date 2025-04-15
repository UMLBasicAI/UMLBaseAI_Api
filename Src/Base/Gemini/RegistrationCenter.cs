using Base.Common.DependencyInjection;
using Base.Config;
using Base.Gemini.Handler;
using Base.Mail.Handler;

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
        });

        services.AddSingleton(geminiOptions);
    }
}
