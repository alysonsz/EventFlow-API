using EventFlow_API.Data.Mapping;
using EventFlow_API.Models;

public class EventFlowContext : DbContext
{
    public EventFlowContext(DbContextOptions<EventFlowContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Event { get; set; } = null!;
    public DbSet<Organizer> Organizer { get; set; } = null!;
    public DbSet<Participant> Participant { get; set; } = null!;
    public DbSet<Speaker> Speaker { get; set; } = null!;
    public DbSet<SpeakerEvent> SpeakerEvents { get; set; } = null!;
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EventMap());
        builder.ApplyConfiguration(new OrganizerMap());
        builder.ApplyConfiguration(new ParticipantMap());
        builder.ApplyConfiguration(new SpeakerMap());
        builder.ApplyConfiguration(new SpeakerEventMap());
    }
}