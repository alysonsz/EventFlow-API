using EventNucleus.Application.DTOs;
using EventNucleus.Core.Models;

namespace EventNucleus.Application.Features.Speakers.Queries.GetAllSpeakers;

public record GetAllSpeakersQuery(QueryParameters QueryParameters) : IQuery<PagedResult<SpeakerDTO>>;

