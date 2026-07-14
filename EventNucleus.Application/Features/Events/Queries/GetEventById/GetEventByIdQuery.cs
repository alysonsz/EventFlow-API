using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Events.Queries.GetEventById;

public record GetEventByIdQuery(int Id) : IQuery<Result<EventDTO>>;

