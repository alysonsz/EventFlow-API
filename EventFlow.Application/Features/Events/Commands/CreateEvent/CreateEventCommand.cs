using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.CreateEvent;

public record CreateEventCommand(
    string Title,
    string? Description,
    DateTime Date,
    string Location,
    int OrganizerId) : ICommand<Result<int>>;
