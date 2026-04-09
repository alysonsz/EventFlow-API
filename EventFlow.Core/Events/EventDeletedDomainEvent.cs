namespace EventFlow.Core.Events;

public class EventDeletedDomainEvent : DomainEvent
{
    public int EventId { get; }
    public string Title { get; }

    public EventDeletedDomainEvent(int eventId, string title)
    {
        EventId = eventId;
        Title = title;
    }
}
