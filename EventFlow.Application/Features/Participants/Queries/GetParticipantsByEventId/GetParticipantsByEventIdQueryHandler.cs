using AutoMapper;
using EventFlow.Application.Abstractions;
using EventFlow.Application.DTOs;
using EventFlow.Core.Models;
using EventFlow.Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventFlow.Application.Features.Participants.Queries.GetParticipantsByEventId;

public class GetParticipantsByEventIdQueryHandler : IRequestHandler<GetParticipantsByEventIdQuery, PagedResult<ParticipantDTO>>
{
    private readonly IParticipantRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ILogger<GetParticipantsByEventIdQueryHandler> _logger;

    public GetParticipantsByEventIdQueryHandler(
        IParticipantRepository repository,
        IMapper mapper,
        ICacheService cache,
        ILogger<GetParticipantsByEventIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResult<ParticipantDTO>> Handle(GetParticipantsByEventIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"participants-event-{request.EventId}-page-{request.QueryParameters.PageNumber}-size-{request.QueryParameters.PageSize}";

        var cached = await _cache.GetAsync<PagedResult<ParticipantDTO>>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for participants of event {EventId} page {PageNumber}", request.EventId, request.QueryParameters.PageNumber);
            return cached;
        }

        _logger.LogInformation("Cache miss for participants of event {EventId} page {PageNumber}. Fetching from database...", request.EventId, request.QueryParameters.PageNumber);

        var pagedResult = await _repository.GetAllPagedParticipantsByEventIdAsync(
            request.EventId,
            request.QueryParameters);

        var dtos = _mapper.Map<List<ParticipantDTO>>(pagedResult.Items);

        var result = new PagedResult<ParticipantDTO>(
            dtos,
            request.QueryParameters.PageNumber,
            request.QueryParameters.PageSize,
            pagedResult.TotalCount);

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(3));
        _logger.LogInformation("Cached participants of event {EventId} page {PageNumber} for 3 minutes", request.EventId, request.QueryParameters.PageNumber);

        return result;
    }
}
