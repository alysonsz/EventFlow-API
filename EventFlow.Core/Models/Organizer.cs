namespace EventFlow.Core.Models;

public class Organizer : Entity<int>
{
    public PersonName Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public ICollection<Event> Events { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Organizer() { }

    public static Organizer Create(string name, string email)
    {
        var organizer = new Organizer
        {
            Name = PersonName.Create(name),
            Email = Email.Create(email),
            CreatedAt = DateTime.UtcNow
        };

        return organizer;
    }

    public void UpdateDetails(string name, string email)
    {
        Name = PersonName.Create(name);
        Email = Email.Create(email);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddEvent(Event @event)
    {
        Events.Add(@event);
    }

    public void RemoveEvent(int eventId)
    {
        var @event = Events.FirstOrDefault(e => e.Id == eventId);
        if (@event != null)
        {
            Events.Remove(@event);
        }
    }
}

