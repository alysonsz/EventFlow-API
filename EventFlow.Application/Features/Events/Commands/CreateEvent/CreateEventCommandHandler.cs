using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<int>>
{
    private readonly IEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(
        IEventRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            request.Title,
            request.Description,
            request.Date,
            request.Location,
            request.OrganizerId);

        await _repository.PostAsync(@event);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(@event.Id);
    }
}
