using AutoMapper;
using EventFlow.Application.Abstractions;
using EventFlow.Application.DTOs;
using EventFlow.Core.Models;
using EventFlow.Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventFlow.Application.Features.Speakers.Queries.GetAllSpeakers;

public class GetAllSpeakersQueryHandler : IRequestHandler<GetAllSpeakersQuery, PagedResult<SpeakerDTO>>
{
    private readonly ISpeakerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ILogger<GetAllSpeakersQueryHandler> _logger;

    public GetAllSpeakersQueryHandler(
        ISpeakerRepository repository,
        IMapper mapper,
        ICacheService cache,
        ILogger<GetAllSpeakersQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResult<SpeakerDTO>> Handle(GetAllSpeakersQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"speakers-page-{request.QueryParameters.PageNumber}-size-{request.QueryParameters.PageSize}";

        var cached = await _cache.GetAsync<PagedResult<SpeakerDTO>>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for speakers page {PageNumber}", request.QueryParameters.PageNumber);
            return cached;
        }

        _logger.LogInformation("Cache miss for speakers page {PageNumber}. Fetching from database...", request.QueryParameters.PageNumber);

        var pagedResult = await _repository.GetAllPagedSpeakersAsync(request.QueryParameters);
        var dtos = _mapper.Map<List<SpeakerDTO>>(pagedResult.Items);

        var result = new PagedResult<SpeakerDTO>(
            dtos,
            request.QueryParameters.PageNumber,
            request.QueryParameters.PageSize,
            pagedResult.TotalCount);

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        _logger.LogInformation("Cached speakers page {PageNumber} for 5 minutes", request.QueryParameters.PageNumber);

        return result;
    }
}
