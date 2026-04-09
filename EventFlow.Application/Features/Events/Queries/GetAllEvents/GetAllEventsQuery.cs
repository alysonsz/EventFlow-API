namespace EventFlow.Application.Features.Events.Queries.GetAllEvents;

public record GetAllEventsQuery(QueryParameters Parameters) : IQuery<PagedResult<EventDTO>>;
