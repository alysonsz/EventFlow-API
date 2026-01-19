using EventFlow.Application.Services;
using EventFlow.Application.Validators;
using EventFlow.Core.Repository.Interfaces;
using EventFlow.Infrastructure.Data;
using EventFlow.Infrastructure.Helpers;
using EventFlow.Infrastructure.Profiles;
using EventFlow.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace EventFlow.Presentation.Config;

public static class AppConfiguration
{
    public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var ConnectionString = configuration.GetConnectionString("DevConnectionString");

        services.AddDbContext<EventFlowContext>(options =>
        {
            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging(true);
                options.EnableDetailedErrors(true);
            }
        });

        return services;
    }

    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IOrganizerRepository, OrganizerRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<ISpeakerRepository, SpeakerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOrganizerService, OrganizerService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        services.AddScoped<ISpeakerService, SpeakerService>();
        services.AddScoped<IAuthService, AuthService>();


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

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "EventFlow API", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Insira o token JWT: Bearer {seu_token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() }
        });
        });

        return services;
    }
}
