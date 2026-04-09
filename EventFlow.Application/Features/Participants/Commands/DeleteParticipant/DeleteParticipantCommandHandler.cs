using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using MediatR;

namespace EventFlow.Application.Features.Participants.Commands.DeleteParticipant;

public class DeleteParticipantCommandHandler : IRequestHandler<DeleteParticipantCommand, Result>
{
    private readonly IParticipantRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public DeleteParticipantCommandHandler(
        IParticipantRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(request.Id);

        if (deleted == 0)
            return Result.Failure(Error.NotFound("Participant.NotFound", $"Participant with id {request.Id} not found"));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"participant-{request.Id}");

        return Result.Success();
    }
}
