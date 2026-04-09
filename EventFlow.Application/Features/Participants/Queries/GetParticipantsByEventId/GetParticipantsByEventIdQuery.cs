using EventFlow.Application.DTOs;
using EventFlow.Core.Models;

namespace EventFlow.Application.Features.Participants.Queries.GetParticipantsByEventId;

public record GetParticipantsByEventIdQuery(int EventId, QueryParameters QueryParameters) : IQuery<PagedResult<ParticipantDTO>>;
