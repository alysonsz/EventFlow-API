using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Queries.GetEventById;

public record GetEventByIdQuery(int Id) : IQuery<Result<EventDTO>>;
