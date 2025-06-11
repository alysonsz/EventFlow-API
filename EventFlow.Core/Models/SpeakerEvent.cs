namespace EventFlow.Core.Models;

public class SpeakerEvent
{
    public int SpeakerId { get; set; }
    public Speaker Speaker { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}

