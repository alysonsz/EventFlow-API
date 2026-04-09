using EventFlow.Application.Abstractions;
using EventFlow.Infrastructure.Data;

namespace EventFlow.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventFlowContext _context;

    public UnitOfWork(EventFlowContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
