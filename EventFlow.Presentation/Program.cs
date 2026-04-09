using EventFlow.Presentation.Config;
using EventFlow.Presentation.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

AppConfiguration.ConfigureMvc(builder);

builder.Services
    .ConfigureSwagger()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContextConfig(builder.Configuration, builder.Environment.IsDevelopment())
    .AddRedisCacheConfig()
    .AddOpenTelemetryConfig()
    .AddDependencyInjectionConfig()
    .AddJwtAuthentication(builder.Configuration)
    .AddRateLimitingConfig()
    .AddHealthCheckConfig(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandler();

app.ApplyDatabaseMigrations();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecksUI();

try
{
    Log.Information("Iniciando Web API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A API caiu inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}