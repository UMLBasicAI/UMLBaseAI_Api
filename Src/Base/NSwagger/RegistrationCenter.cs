using Base.Common.DependencyInjection;
using Base.Config;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Base.NSwagger;

internal sealed class RegistrationCenter : IExternalServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureSwagger(services, configuration);
        return services;
    }

    private static IServiceCollection ConfigureSwagger(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        const string JWT_BEARER_SCHEME_NAME = "JWT Bearer";
        var swaggerOptions = configuration
            .GetRequiredSection(key: "Swagger")
            .GetRequiredSection("NSwag")
            .Get<NSwagOption>();

        services.AddOpenApiDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info = new()
                {
                    Version = swaggerOptions?.Doc.PostProcess.Info.Version,
                    Title = swaggerOptions?.Doc.PostProcess.Info.Title,
                    Description = swaggerOptions?.Doc.PostProcess.Info.Description,
                    Contact = new()
                    {
                        Name = swaggerOptions?.Doc.PostProcess.Info.Contact.Name,
                        Email = swaggerOptions?.Doc.PostProcess.Info.Contact.Email,
                    },
                    License = new()
                    {
                        Name = swaggerOptions?.Doc.PostProcess.Info.License.Name,
                        Url = new(swaggerOptions?.Doc.PostProcess.Info.License.Url),
                    },
                };
            };

            config.AddSecurity(
                JWT_BEARER_SCHEME_NAME,
                new NSwag.OpenApiSecurityScheme()
                {
                    Type = (OpenApiSecuritySchemeType)
                        Enum.ToObject(
                            typeof(OpenApiSecuritySchemeType),
                            swaggerOptions!.Doc.Auth.Bearer.Type
                        ),
                    In = (OpenApiSecurityApiKeyLocation)
                        Enum.ToObject(
                            typeof(OpenApiSecurityApiKeyLocation),
                            swaggerOptions.Doc.Auth.Bearer.In
                        ),
                    Scheme = swaggerOptions.Doc.Auth.Bearer.Scheme,
                    BearerFormat = swaggerOptions.Doc.Auth.Bearer.BearerFormat,
                    Description = swaggerOptions.Doc.Auth.Bearer.Description,
                }
            );

            config.OperationProcessors.Add(
                new OperationSecurityScopeProcessor(JWT_BEARER_SCHEME_NAME)
            );

            //config.SchemaSettings.SchemaProcessors.Add(new IgnoreEmptyRequestSchemaProcessor());
        });

        return services;
    }
}
