using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Participants.Commands.UpdateParticipant;

public record UpdateParticipantCommand(
    int Id,
    string Name,
    string? Email,
    string? Interests) : ICommand<Result>;
