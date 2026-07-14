using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Participants.Commands.DeleteParticipant;

public record DeleteParticipantCommand(int Id) : ICommand<Result>;

