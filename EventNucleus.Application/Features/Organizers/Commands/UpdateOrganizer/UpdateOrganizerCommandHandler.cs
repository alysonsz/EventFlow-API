using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Organizers.Commands.UpdateOrganizer;

public class UpdateOrganizerCommandHandler : IRequestHandler<UpdateOrganizerCommand, Result>
{
    private readonly IOrganizerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public UpdateOrganizerCommandHandler(
        IOrganizerRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _repository.GetOrganizerByIdAsync(request.Id);

        if (organizer == null)
            return Result.Failure(Error.NotFound("Organizer.NotFound", $"Organizer with id {request.Id} not found"));

        organizer.UpdateDetails(request.Name, request.Email ?? "");

        await _repository.UpdateAsync(organizer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"organizer-{request.Id}");

        return Result.Success();
    }
}

