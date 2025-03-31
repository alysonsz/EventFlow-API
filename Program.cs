using EventFlow_API.Config;

var builder = WebApplication.CreateBuilder(args);

AppConfiguration.ConfigureMvc(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextConfig(builder.Configuration);
builder.Services.AddDependencyInjectionConfig();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
