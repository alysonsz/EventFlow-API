using EventNucleus.Application.DTOs;
using EventNucleus.Core.Models;

namespace EventNucleus.Application.Features.Participants.Queries.GetParticipantsByEventId;

public record GetParticipantsByEventIdQuery(int EventId, QueryParameters QueryParameters) : IQuery<PagedResult<ParticipantDTO>>;

