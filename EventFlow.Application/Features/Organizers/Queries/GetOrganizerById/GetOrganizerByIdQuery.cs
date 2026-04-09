using EventFlow.Application.DTOs;
using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Organizers.Queries.GetOrganizerById;

public record GetOrganizerByIdQuery(int Id) : IQuery<Result<OrganizerDTO>>;
