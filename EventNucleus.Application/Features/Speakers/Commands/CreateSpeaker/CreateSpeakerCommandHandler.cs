using EventNucleus.Core.Models;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Speakers.Commands.CreateSpeaker;

public class CreateSpeakerCommandHandler : IRequestHandler<CreateSpeakerCommand, Result<int>>
{
    private readonly ISpeakerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeakerCommandHandler(
        ISpeakerRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateSpeakerCommand request, CancellationToken cancellationToken)
    {
        var speaker = Speaker.Create(
            request.Name,
            request.Email ?? "",
            request.Biography ?? "",
            request.ExpertiseArea ?? "");

        await _repository.PostAsync(speaker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(speaker.Id);
    }
}

