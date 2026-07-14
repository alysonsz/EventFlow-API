using EventNucleus.Application.DTOs;
using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Participants.Queries.GetParticipantById;

public record GetParticipantByIdQuery(int Id) : IQuery<Result<ParticipantDTO>>;

