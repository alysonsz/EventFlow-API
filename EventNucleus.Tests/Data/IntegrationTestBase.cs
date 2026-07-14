using EventNucleus.Infrastructure.Data;
using Microsoft.Data.Sqlite;

namespace EventNucleus_API.Tests.Data;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly EventNucleusContext _context;
    private readonly SqliteConnection _connection;

    protected IntegrationTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<EventNucleusContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new EventNucleusContext(options);
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



