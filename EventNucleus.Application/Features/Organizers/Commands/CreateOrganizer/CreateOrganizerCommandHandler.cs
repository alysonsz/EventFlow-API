using EventNucleus.Core.Models;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using MediatR;

namespace EventNucleus.Application.Features.Organizers.Commands.CreateOrganizer;

public class CreateOrganizerCommandHandler : IRequestHandler<CreateOrganizerCommand, Result<int>>
{
    private readonly IOrganizerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrganizerCommandHandler(
        IOrganizerRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = Organizer.Create(request.Name, request.Email ?? "");

        await _repository.PostAsync(organizer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(organizer.Id);
    }
}

