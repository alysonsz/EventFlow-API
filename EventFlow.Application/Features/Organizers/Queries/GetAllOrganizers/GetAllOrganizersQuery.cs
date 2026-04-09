using EventFlow.Application.DTOs;
using EventFlow.Core.Models;

namespace EventFlow.Application.Features.Organizers.Queries.GetAllOrganizers;

public record GetAllOrganizersQuery(QueryParameters QueryParameters) : IQuery<PagedResult<OrganizerDTO>>;
