using EventFlow.Infrastructure.Data;
using Microsoft.Data.Sqlite;

namespace EventFlow_API.Tests.Data;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly EventFlowContext _context;
    private readonly SqliteConnection _connection;

    protected IntegrationTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<EventFlowContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new EventFlowContext(options);
        _context.Database.EnsureCreated();

        SeedData();
    }

    private void SeedData()
    {
        var organizer = Organizer.Create("Organizer Test", "test@test.com");
        typeof(Organizer).GetProperty("Id")!.SetValue(organizer, 1);
        _context.Organizer.Add(organizer);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _connection?.Dispose();
    }
}


