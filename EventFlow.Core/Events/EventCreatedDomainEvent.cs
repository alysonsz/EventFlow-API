namespace EventFlow.Core.Events;

public class EventCreatedDomainEvent : DomainEvent
{
    public int EventId { get; }
    public string Title { get; }
    public DateTime Date { get; }
    public int OrganizerId { get; }

    public EventCreatedDomainEvent(int eventId, string title, DateTime date, int organizerId)
    {
        EventId = eventId;
        Title = title;
        Date = date;
        OrganizerId = organizerId;
    }
}
