using EventFlow.Application.DTOs;
using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Speakers.Queries.GetSpeakerById;

public record GetSpeakerByIdQuery(int Id) : IQuery<Result<SpeakerDTO>>;
