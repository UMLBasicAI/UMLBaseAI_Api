using System.Reflection;
using System.Xml.Linq;
using Base.Common.DependencyInjection;

namespace Entry.Registry;

internal static class AppServiceRegisterationCenter
{
    private static readonly Type ServiceRegisterType = typeof(IServiceRegister);

    internal static IServiceCollection RegisterRequireServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        //Entry service registry
        services = RegistrationCenter.Register(services, configuration);
        //Base service registry
        services = Base.RegistrationCenter.Register(services, configuration);
        //Core services registry
        var registeredAssemblyNames = GetListOfRegisteredAssemblyNameAsync(configuration);
        services = RegisterAssemblyByName(registeredAssemblyNames!, services, configuration);
        return services;
    }

    private static IServiceCollection RegisterAssemblyByName(
        IEnumerable<string> AssemblyNames,
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        foreach (var assemblyName in AssemblyNames)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
                throw new ApplicationException(
                    $"No assembly {assemblyName} is found, please check !!"
                );
            }

            var allTypes = assembly.GetTypes();
            var isRegisterTypeFound = allTypes.Count(type =>
                ServiceRegisterType.IsAssignableFrom(type) && !type.IsInterface
            );
            if (isRegisterTypeFound < 1)
            {
                throw new ApplicationException(
                    $"No classes that inherit {nameof(IServiceRegister)} interface are found in this assembly {assembly.GetName()}, please check again !!"
                );
            }
            if (isRegisterTypeFound > 1)
            {
                throw new ApplicationException(
                    $"Only 1 class that inherits {nameof(IServiceRegister)} interface can exist in this assembly {assembly.GetName()}, please check again !!"
                );
            }

            var registerType = allTypes.First(type =>
                ServiceRegisterType.IsAssignableFrom(type) && !type.IsInterface
            );
            var register = (IServiceRegister)Activator.CreateInstance(registerType)!;
            register.Register(services, configuration);
        }
        return services;
    }

    private static IEnumerable<string?> GetListOfRegisteredAssemblyNameAsync(
        IConfiguration configuration
    )
    {
        var options = configuration
            .GetRequiredSection("ProjectReferences")
            .Get<IEnumerable<string>>();
        return options;
    }
}
