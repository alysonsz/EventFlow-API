using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.UpdateEvent;

public record UpdateEventCommand(
    int Id,
    string Title,
    string? Description,
    DateTime Date,
    string Location,
    int OrganizerId) : ICommand<Result>;
