using AutoMapper;
using EventNucleus.Application.DTOs;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Speakers.Queries.GetSpeakerById;

public class GetSpeakerByIdQueryHandler : IRequestHandler<GetSpeakerByIdQuery, Result<SpeakerDTO>>
{
    private readonly ISpeakerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetSpeakerByIdQueryHandler(
        ISpeakerRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<SpeakerDTO>> Handle(GetSpeakerByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"speaker-{request.Id}";

        var cachedData = await _cache.GetAsync<SpeakerDTO>(cacheKey);
        if (cachedData != null)
            return Result<SpeakerDTO>.Success(cachedData);

        var speaker = await _repository.GetSpeakerByIdAsync(request.Id);

        if (speaker == null)
            return Result<SpeakerDTO>.Failure(Error.NotFound("Speaker.NotFound", $"Speaker with id {request.Id} not found"));

        var dto = _mapper.Map<SpeakerDTO>(speaker);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));

        return Result<SpeakerDTO>.Success(dto);
    }
}

