using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Organizers.Commands.DeleteOrganizer;

public class DeleteOrganizerCommandHandler : IRequestHandler<DeleteOrganizerCommand, Result>
{
    private readonly IOrganizerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public DeleteOrganizerCommandHandler(
        IOrganizerRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteOrganizerCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(request.Id);

        if (deleted == 0)
            return Result.Failure(Error.NotFound("Organizer.NotFound", $"Organizer with id {request.Id} not found"));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"organizer-{request.Id}");

        return Result.Success();
    }
}

