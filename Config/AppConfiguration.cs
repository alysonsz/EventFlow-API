using EventFlow_API.Repository;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;
using EventFlow_API.Services;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using EventFlow_API.Helpers;
using EventFlow_API.Validators;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using EventFlow_API.Profiles;

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

        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOrganizerService, OrganizerService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        services.AddScoped<ISpeakerService, SpeakerService>();


        return services;
    }

    public static void ConfigureMvc(WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCompression(options =>
            options.Providers.Add<GzipCompressionProvider>());

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Optimal);

        builder.Services
            .AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new DateTimeConverterHelper());
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<EventCommandValidator>()
            .AddValidatorsFromAssemblyContaining<SpeakerCommandValidator>()
            .AddValidatorsFromAssemblyContaining<OrganizerCommandValidator>()
            .AddValidatorsFromAssemblyContaining<ParticipantCommandValidator>();
    }
}
