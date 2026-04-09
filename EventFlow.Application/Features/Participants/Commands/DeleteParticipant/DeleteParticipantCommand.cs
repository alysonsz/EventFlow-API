using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Participants.Commands.DeleteParticipant;

public record DeleteParticipantCommand(int Id) : ICommand<Result>;
