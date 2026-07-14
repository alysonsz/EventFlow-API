using EventNucleus.Application.DTOs;
using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Organizers.Queries.GetOrganizerById;

public record GetOrganizerByIdQuery(int Id) : IQuery<Result<OrganizerDTO>>;

