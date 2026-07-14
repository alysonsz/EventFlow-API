using AutoMapper;
using EventNucleus.Application.Abstractions;
using EventNucleus.Application.DTOs;
using EventNucleus.Core.Models;
using EventNucleus.Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventNucleus.Application.Features.Organizers.Queries.GetAllOrganizers;

public class GetAllOrganizersQueryHandler : IRequestHandler<GetAllOrganizersQuery, PagedResult<OrganizerDTO>>
{
    private readonly IOrganizerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ILogger<GetAllOrganizersQueryHandler> _logger;

    public GetAllOrganizersQueryHandler(
        IOrganizerRepository repository,
        IMapper mapper,
        ICacheService cache,
        ILogger<GetAllOrganizersQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResult<OrganizerDTO>> Handle(GetAllOrganizersQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"organizers-page-{request.QueryParameters.PageNumber}-size-{request.QueryParameters.PageSize}";

        var cached = await _cache.GetAsync<PagedResult<OrganizerDTO>>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for organizers page {PageNumber}", request.QueryParameters.PageNumber);
            return cached;
        }

        _logger.LogInformation("Cache miss for organizers page {PageNumber}. Fetching from database...", request.QueryParameters.PageNumber);

        var pagedResult = await _repository.GetAllPagedOrganizersAsync(request.QueryParameters);
        var dtos = _mapper.Map<List<OrganizerDTO>>(pagedResult.Items);

        var result = new PagedResult<OrganizerDTO>(
            dtos,
            request.QueryParameters.PageNumber,
            request.QueryParameters.PageSize,
            pagedResult.TotalCount);

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        _logger.LogInformation("Cached organizers page {PageNumber} for 5 minutes", request.QueryParameters.PageNumber);

        return result;
    }
}

