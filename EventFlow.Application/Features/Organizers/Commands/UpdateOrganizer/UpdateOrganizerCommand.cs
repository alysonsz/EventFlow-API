using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Organizers.Commands.UpdateOrganizer;

public record UpdateOrganizerCommand(
    int Id,
    string Name,
    string? Email) : ICommand<Result>;
