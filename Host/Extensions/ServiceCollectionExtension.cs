
using System.Reflection;
using Mapster;
using MapsterMapper;
using PingCrm.Host.Data.Services;

namespace PingCrm.Host.Extensions;


public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        // https://stackoverflow.com/questions/61172885/mapster-global-configuration-with-dependency-injection

        var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);
        // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        config.Scan(Assembly.GetExecutingAssembly());
        // register the mapper as Singleton service for my application
        var mapperConfig = new Mapper(config);
        services.AddSingleton<IMapper>(mapperConfig);
        return services;
    }


    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<ContactService>();
        services.AddScoped<OrganizationService>();
        services.AddScoped<UserService>();
        return services;
    }
}