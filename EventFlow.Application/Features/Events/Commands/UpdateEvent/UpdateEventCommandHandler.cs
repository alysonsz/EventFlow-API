using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Result>
{
    private readonly IEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public UpdateEventCommandHandler(
        IEventRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _repository.GetEventByIdAsync(request.Id);
        
        if (@event == null)
            return Result.Failure(Error.NotFound("Event.NotFound", $"Event with id {request.Id} not found"));

        @event.UpdateDetails(
            request.Title,
            request.Description,
            request.Date,
            request.Location);

        await _repository.UpdateAsync(@event);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"event-{request.Id}");

        return Result.Success();
    }
}
