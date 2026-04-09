using AutoMapper;
using EventFlow.Application.DTOs;
using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using MediatR;

namespace EventFlow.Application.Features.Participants.Queries.GetParticipantById;

public class GetParticipantByIdQueryHandler : IRequestHandler<GetParticipantByIdQuery, Result<ParticipantDTO>>
{
    private readonly IParticipantRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetParticipantByIdQueryHandler(
        IParticipantRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<ParticipantDTO>> Handle(GetParticipantByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"participant-{request.Id}";

        var cachedData = await _cache.GetAsync<ParticipantDTO>(cacheKey);
        if (cachedData != null)
            return Result<ParticipantDTO>.Success(cachedData);

        var participant = await _repository.GetParticipantByIdAsync(request.Id);

        if (participant == null)
            return Result<ParticipantDTO>.Failure(Error.NotFound("Participant.NotFound", $"Participant with id {request.Id} not found"));

        var dto = _mapper.Map<ParticipantDTO>(participant);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));

        return Result<ParticipantDTO>.Success(dto);
    }
}
