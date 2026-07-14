using EventNucleus.Core.Primitives;
using EventNucleus.Infrastructure.Data.Mapping;

namespace EventNucleus.Infrastructure.Data;

public class EventNucleusContext : DbContext
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    public EventNucleusContext(DbContextOptions<EventNucleusContext> options) : base(options)
    {
    }

    public EventNucleusContext(
        DbContextOptions<EventNucleusContext> options,
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

        if (_domainEventDispatcher != null && domainEvents.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
        }

        return result;
    }
}
