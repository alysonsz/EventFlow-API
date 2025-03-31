using EventFlow_API.Repository;
using EventFlow_API.Repository.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace EventFlow_API.Config;

public static class AppConfiguration
{
    public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var ConnectionString = configuration.GetConnectionString("DevConnectionString");

        services.AddDbContext<EventFlowContext>(options =>
        {
            options.UseSqlServer(ConnectionString);
            options.EnableSensitiveDataLogging(true);
        });

        return services;
    }

    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IOrganizerRepository, OrganizerRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<ISpeakerRepository, SpeakerRepository>();

        return services;
    }

    public static void ConfigureMvc(WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCompression(options =>
            options.Providers.Add<GzipCompressionProvider>());
        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Optimal);
        builder
            .Services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }
}
