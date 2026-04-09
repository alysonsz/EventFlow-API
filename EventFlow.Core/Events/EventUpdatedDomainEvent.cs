namespace EventFlow.Core.Events;

public class EventUpdatedDomainEvent : DomainEvent
{
    public int EventId { get; }
    public string Title { get; }
    public DateTime Date { get; }
    public string Location { get; }

    public EventUpdatedDomainEvent(int eventId, string title, DateTime date, string location)
    {
        EventId = eventId;
        Title = title;
        Date = date;
        Location = location;
    }
}
