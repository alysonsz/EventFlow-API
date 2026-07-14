namespace EventNucleus.Application.Features.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, PagedResult<EventDTO>>
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;

    public GetAllEventsQueryHandler(IEventRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<EventDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var pagedEvents = await _repository.GetAllPagedEventsAsync(request.Parameters);

        var eventDtos = _mapper.Map<List<EventDTO>>(pagedEvents.Items);

        return new PagedResult<EventDTO>(
            eventDtos,
            pagedEvents.PageNumber,
            pagedEvents.PageSize,
            pagedEvents.TotalCount
        );
    }
}

