using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Participants.Commands.CreateParticipant;

public record CreateParticipantCommand(
    string Name,
    string? Email,
    string? Interests) : ICommand<Result<int>>;
