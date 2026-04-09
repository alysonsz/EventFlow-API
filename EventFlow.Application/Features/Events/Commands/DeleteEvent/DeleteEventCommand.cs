using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Events.Commands.DeleteEvent;

public record DeleteEventCommand(int Id) : ICommand<Result>;
