using EventFlow.Application.DTOs;
using EventFlow.Core.Models;

namespace EventFlow.Application.Features.Speakers.Queries.GetAllSpeakers;

public record GetAllSpeakersQuery(QueryParameters QueryParameters) : IQuery<PagedResult<SpeakerDTO>>;
