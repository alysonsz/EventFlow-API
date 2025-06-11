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
        _context.Organizer.Add(new Organizer { Id = 1, Name = "Organizer Test", Email = "test@test.com" });
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _connection?.Dispose();
    }
}


