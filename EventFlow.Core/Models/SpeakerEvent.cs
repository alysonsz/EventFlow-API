namespace EventFlow.Core.Models;

public class SpeakerEvent
{
    public int SpeakerId { get; internal set; }
    public Speaker Speaker { get; internal set; } = null!;
    public int EventId { get; internal set; }
    public Event Event { get; internal set; } = null!;
    public DateTime RegisteredAt { get; private set; }

    public static SpeakerEvent Create(Speaker speaker, Event @event)
    {
        return new SpeakerEvent
        {
            SpeakerId = speaker.Id,
            Speaker = speaker,
            EventId = @event.Id,
            Event = @event,
            RegisteredAt = DateTime.UtcNow
        };
    }
}

