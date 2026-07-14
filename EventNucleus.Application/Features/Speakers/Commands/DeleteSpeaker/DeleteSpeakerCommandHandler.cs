using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Speakers.Commands.DeleteSpeaker;

public class DeleteSpeakerCommandHandler : IRequestHandler<DeleteSpeakerCommand, Result>
{
    private readonly ISpeakerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public DeleteSpeakerCommandHandler(
        ISpeakerRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteSpeakerCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(request.Id);

        if (deleted == 0)
            return Result.Failure(Error.NotFound("Speaker.NotFound", $"Speaker with id {request.Id} not found"));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"speaker-{request.Id}");

        return Result.Success();
    }
}

