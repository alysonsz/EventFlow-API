using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using MediatR;

namespace EventFlow.Application.Features.Speakers.Commands.UpdateSpeaker;

public class UpdateSpeakerCommandHandler : IRequestHandler<UpdateSpeakerCommand, Result>
{
    private readonly ISpeakerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public UpdateSpeakerCommandHandler(
        ISpeakerRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateSpeakerCommand request, CancellationToken cancellationToken)
    {
        var speaker = await _repository.GetSpeakerByIdAsync(request.Id);

        if (speaker == null)
            return Result.Failure(Error.NotFound("Speaker.NotFound", $"Speaker with id {request.Id} not found"));

        speaker.UpdateDetails(
            request.Name,
            request.Email ?? "",
            request.Biography ?? "",
            request.ExpertiseArea ?? "");

        await _repository.UpdateAsync(speaker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"speaker-{request.Id}");

        return Result.Success();
    }
}
