namespace EventNucleus.Core.Models;

public class Speaker : Entity<int>
{
    public PersonName Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string? Biography { get; private set; }
    public string? Expertise { get; private set; }
    public ICollection<SpeakerEvent> SpeakerEvents { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Speaker() { }

    public static Speaker Create(string name, string email, string? biography = null, string? expertise = null)
    {
        var speaker = new Speaker
        {
            Name = PersonName.Create(name),
            Email = Email.Create(email),
            Biography = biography,
            Expertise = expertise,
            CreatedAt = DateTime.UtcNow
        };

        return speaker;
    }

    public void UpdateDetails(string name, string email, string? biography, string? expertise)
    {
        Name = PersonName.Create(name);
        Email = Email.Create(email);
        Biography = biography;
        Expertise = expertise;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBiography(string biography)
    {
        Biography = biography;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateExpertise(string expertise)
    {
        Expertise = expertise;
        UpdatedAt = DateTime.UtcNow;
    }

    public void LinkToEvent(SpeakerEvent speakerEvent)
    {
        if (SpeakerEvents.Any(se => se.EventId == speakerEvent.EventId))
            throw new InvalidOperationException("Already linked to this event");

        SpeakerEvents.Add(speakerEvent);
    }

    public void UnlinkFromEvent(int eventId)
    {
        var speakerEvent = SpeakerEvents.FirstOrDefault(se => se.EventId == eventId);
        if (speakerEvent != null)
        {
            SpeakerEvents.Remove(speakerEvent);
        }
    }
}


