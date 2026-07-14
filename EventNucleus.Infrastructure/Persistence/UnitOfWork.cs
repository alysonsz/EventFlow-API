using EventNucleus.Application.Abstractions;
using EventNucleus.Infrastructure.Data;

namespace EventNucleus.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventNucleusContext _context;

    public UnitOfWork(EventNucleusContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}

