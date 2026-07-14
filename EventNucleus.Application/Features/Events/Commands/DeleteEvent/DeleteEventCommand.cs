using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Events.Commands.DeleteEvent;

public record DeleteEventCommand(int Id) : ICommand<Result>;

