using EventNucleus.Application.DTOs;
using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Speakers.Queries.GetSpeakerById;

public record GetSpeakerByIdQuery(int Id) : IQuery<Result<SpeakerDTO>>;

