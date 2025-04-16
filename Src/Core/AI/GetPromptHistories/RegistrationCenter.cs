using Base.Common.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GetPromptHistories.BusinessLogic;
using GetPromptHistories.DataAccess;

namespace GetPromptHistories;

public class RegistrationCenter : IServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = typeof(RegistrationCenter).Assembly;
        #region Filters
        services.RegisterFilterFromAssembly(currentAssembly);
        #endregion

        #region Validation
        services.AddValidatorsFromAssembly(currentAssembly, ServiceLifetime.Singleton);
        #endregion

        #region Core
        services
            .AddScoped<IRepository, Repository>()
            .MakeScopedLazy<IRepository>()
            .AddScoped<Service>();
        #endregion

        return services;
    }
}
