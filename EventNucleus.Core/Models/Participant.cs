namespace EventNucleus.Core.Models;

public class Participant : Entity<int>
{
    public PersonName Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string? Interests { get; private set; }
    public ICollection<Event> Events { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Participant() { }

    public static Participant Create(string name, string email, string? interests = null)
    {
        var participant = new Participant
        {
            Name = PersonName.Create(name),
            Email = Email.Create(email),
            Interests = interests,
            CreatedAt = DateTime.UtcNow
        };

        return participant;
    }

    public void UpdateDetails(string name, string email, string? interests)
    {
        Name = PersonName.Create(name);
        Email = Email.Create(email);
        Interests = interests;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateInterests(string interests)
    {
        Interests = interests;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterForEvent(Event @event)
    {
        if (Events.Any(e => e.Id == @event.Id))
            throw new InvalidOperationException("Already registered for this event");

        Events.Add(@event);
    }

    public void UnregisterFromEvent(int eventId)
    {
        var @event = Events.FirstOrDefault(e => e.Id == eventId);
        if (@event != null)
        {
            Events.Remove(@event);
        }
    }
}


