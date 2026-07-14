namespace EventNucleus.Core.Models;

public class Event : Entity<int>
{
    public EventTitle Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime Date { get; private set; }
    public EventLocation Location { get; private set; } = null!;
    public int OrganizerId { get; private set; }
    public Organizer? Organizer { get; init; }
    public string? Category { get; init; }
    public ICollection<SpeakerEvent> SpeakerEvents { get; private set; } = [];
    public ICollection<Participant> Participants { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Event() { }

    public static Event Create(string title, string? description, DateTime date, string location, int organizerId)
    {
        CheckRule(new EventDateMustBeInFutureRule(date));

        var @event = new Event
        {
            Title = EventTitle.Create(title),
            Description = description,
            Date = date,
            Location = EventLocation.Create(location),
            OrganizerId = organizerId,
            CreatedAt = DateTime.UtcNow
        };

        @event.AddDomainEvent(new EventCreatedDomainEvent(@event.Id, title, date, organizerId));

        return @event;
    }

    public void UpdateDetails(string title, string? description, DateTime date, string location)
    {
        CheckRule(new EventDateMustBeInFutureRule(date));

        Title = EventTitle.Create(title);
        Description = description;
        Date = date;
        Location = EventLocation.Create(location);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new EventUpdatedDomainEvent(Id, title, date, location));
    }

    public void Reschedule(DateTime newDate)
    {
        CheckRule(new EventDateMustBeInFutureRule(newDate));

        Date = newDate;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new EventUpdatedDomainEvent(Id, Title.Value, newDate, Location.FullAddress));
    }

    public void ChangeLocation(string newLocation)
    {
        Location = EventLocation.Create(newLocation);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new EventUpdatedDomainEvent(Id, Title.Value, Date, newLocation));
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new EventDeletedDomainEvent(Id, Title.Value));
    }

    public void AddSpeaker(Speaker speaker)
    {
        if (SpeakerEvents.Any(se => se.SpeakerId == speaker.Id))
            throw new InvalidOperationException("Speaker already added to this event");

        var speakerEvent = SpeakerEvent.Create(speaker, this);
        SpeakerEvents.Add(speakerEvent);
    }

    public void RemoveSpeaker(int speakerId)
    {
        var speakerEvent = SpeakerEvents.FirstOrDefault(se => se.SpeakerId == speakerId);
        if (speakerEvent != null)
        {
            SpeakerEvents.Remove(speakerEvent);
        }
    }

    public void AddParticipant(Participant participant)
    {
        if (Participants.Any(p => p.Id == participant.Id))
            throw new InvalidOperationException("Participant already registered for this event");

        Participants.Add(participant);
    }

    public void RemoveParticipant(int participantId)
    {
        var participant = Participants.FirstOrDefault(p => p.Id == participantId);
        if (participant != null)
        {
            Participants.Remove(participant);
        }
    }
}

public class EventDateMustBeInFutureRule : IBusinessRule
{
    private readonly DateTime _date;
    private static readonly TimeSpan ClockSkew = TimeSpan.FromMinutes(5);

    public EventDateMustBeInFutureRule(DateTime date)
    {
        _date = date;
    }

    public bool IsBroken() => _date < DateTime.UtcNow.Subtract(ClockSkew);
    public string Message => "Event date must be in the future";
}


