using EventNucleus.Application.DTOs;
using EventNucleus.Core.Models;

namespace EventNucleus.Application.Features.Organizers.Queries.GetAllOrganizers;

public record GetAllOrganizersQuery(QueryParameters QueryParameters) : IQuery<PagedResult<OrganizerDTO>>;

