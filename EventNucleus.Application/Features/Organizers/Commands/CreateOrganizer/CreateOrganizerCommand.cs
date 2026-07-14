using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Organizers.Commands.CreateOrganizer;

public record CreateOrganizerCommand(
    string Name,
    string? Email) : ICommand<Result<int>>;

