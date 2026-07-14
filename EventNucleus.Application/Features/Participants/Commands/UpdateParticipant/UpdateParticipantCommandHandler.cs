using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Participants.Commands.UpdateParticipant;

public class UpdateParticipantCommandHandler : IRequestHandler<UpdateParticipantCommand, Result>
{
    private readonly IParticipantRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public UpdateParticipantCommandHandler(
        IParticipantRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
    {
        var participant = await _repository.GetParticipantByIdAsync(request.Id);

        if (participant == null)
            return Result.Failure(Error.NotFound("Participant.NotFound", $"Participant with id {request.Id} not found"));

        participant.UpdateDetails(
            request.Name,
            request.Email ?? "",
            request.Interests ?? "");

        await _repository.UpdateAsync(participant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"participant-{request.Id}");

        return Result.Success();
    }
}

