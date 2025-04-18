﻿using Base.Common.DependencyInjection;

namespace Entry.Registry;

internal static class RegistrationCenter
{
    internal static IServiceCollection Register(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddLogging(config =>
        {
            config.ClearProviders().AddConsole();
        });

        services.AddControllers(config =>
        {
            config.SuppressAsyncSuffixInActionNames = false;
        });

        services.AddCors(config =>
            config.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            )
        );

        services.AddHttpContextAccessor().MakeSingletonLazy<IHttpContextAccessor>();

        return services;
    }
}
