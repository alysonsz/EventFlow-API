using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.UpdateEvent;

public record UpdateEventCommand(
    int Id,
    string Title,
    string? Description,
    DateTime Date,
    string Location) : ICommand<Result>;
