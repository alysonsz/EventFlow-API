using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Organizers.Commands.UpdateOrganizer;

public record UpdateOrganizerCommand(
    int Id,
    string Name,
    string? Email) : ICommand<Result>;

