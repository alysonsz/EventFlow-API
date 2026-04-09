using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Result<EventDTO>>
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetEventByIdQueryHandler(
        IEventRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<EventDTO>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"event-{request.Id}";

        var cached = await _cache.GetAsync<EventDTO>(cacheKey);
        if (cached != null)
            return Result<EventDTO>.Success(cached);

        var @event = await _repository.GetEventWithDetailsByIdAsync(request.Id);
        
        if (@event == null)
            return Result<EventDTO>.Failure(Error.NotFound("Event.NotFound", $"Event with id {request.Id} not found"));

        var dto = _mapper.Map<EventDTO>(@event);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));

        return Result<EventDTO>.Success(dto);
    }
}
