using EventNucleus.Core.Models;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Participants.Commands.CreateParticipant;

public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommand, Result<int>>
{
    private readonly IParticipantRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParticipantCommandHandler(
        IParticipantRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
    {
        var participant = Participant.Create(
            request.Name,
            request.Email ?? "",
            request.Interests ?? "");

        await _repository.PostAsync(participant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(participant.Id);
    }
}

