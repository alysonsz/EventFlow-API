using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Result>
{
    private readonly IEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public DeleteEventCommandHandler(
        IEventRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _repository.GetEventByIdAsync(request.Id);
        
        if (@event == null)
            return Result.Failure(Error.NotFound("Event.NotFound", $"Event with id {request.Id} not found"));

        @event.MarkAsDeleted();
        
        var deleted = await _repository.DeleteAsync(request.Id);
        
        if (deleted > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync($"event-{request.Id}");
            return Result.Success();
        }

        return Result.Failure(Error.Failure("Event.DeleteFailed", "Failed to delete event"));
    }
}

