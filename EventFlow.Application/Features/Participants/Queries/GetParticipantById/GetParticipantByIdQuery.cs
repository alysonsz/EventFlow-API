using EventFlow.Application.DTOs;
using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Participants.Queries.GetParticipantById;

public record GetParticipantByIdQuery(int Id) : IQuery<Result<ParticipantDTO>>;
