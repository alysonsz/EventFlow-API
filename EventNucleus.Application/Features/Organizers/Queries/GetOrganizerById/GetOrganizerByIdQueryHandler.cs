using AutoMapper;
using EventNucleus.Application.DTOs;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Organizers.Queries.GetOrganizerById;

public class GetOrganizerByIdQueryHandler : IRequestHandler<GetOrganizerByIdQuery, Result<OrganizerDTO>>
{
    private readonly IOrganizerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetOrganizerByIdQueryHandler(
        IOrganizerRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<OrganizerDTO>> Handle(GetOrganizerByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"organizer-{request.Id}";

        var cachedData = await _cache.GetAsync<OrganizerDTO>(cacheKey);
        if (cachedData != null)
            return Result<OrganizerDTO>.Success(cachedData);

        var organizer = await _repository.GetOrganizerByIdAsync(request.Id);

        if (organizer == null)
            return Result<OrganizerDTO>.Failure(Error.NotFound("Organizer.NotFound", $"Organizer with id {request.Id} not found"));

        var dto = _mapper.Map<OrganizerDTO>(organizer);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));

        return Result<OrganizerDTO>.Success(dto);
    }
}

