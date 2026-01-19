using EventFlow.Presentation.Config;
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
    .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.ApplyDatabaseMigrations();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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