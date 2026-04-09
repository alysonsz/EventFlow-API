using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Organizers.Commands.CreateOrganizer;

public record CreateOrganizerCommand(
    string Name,
    string? Email) : ICommand<Result<int>>;
