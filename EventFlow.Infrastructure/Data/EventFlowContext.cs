using EventFlow.Core.Primitives;
using EventFlow.Infrastructure.Data.Mapping;

namespace EventFlow.Infrastructure.Data;

public class EventFlowContext : DbContext
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    public EventFlowContext(DbContextOptions<EventFlowContext> options) : base(options)
    {
    }

    public EventFlowContext(
        DbContextOptions<EventFlowContext> options,
        IDomainEventDispatcher domainEventDispatcher) : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<Event> Event { get; set; } = null!;
    public DbSet<Organizer> Organizer { get; set; } = null!;
    public DbSet<Participant> Participant { get; set; } = null!;
    public DbSet<Speaker> Speaker { get; set; } = null!;
    public DbSet<SpeakerEvent> SpeakerEvents { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EventMap());
        builder.ApplyConfiguration(new OrganizerMap());
        builder.ApplyConfiguration(new ParticipantMap());
        builder.ApplyConfiguration(new SpeakerMap());
        builder.ApplyConfiguration(new SpeakerEventMap());
        builder.ApplyConfiguration(new UserMap());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        ChangeTracker
            .Entries<IHasDomainEvents>()
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_domainEventDispatcher != null && domainEvents.Any())
        {
            await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
        }

        return result;
    }
}