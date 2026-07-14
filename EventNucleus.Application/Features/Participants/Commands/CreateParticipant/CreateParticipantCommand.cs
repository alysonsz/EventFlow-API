using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Participants.Commands.CreateParticipant;

public record CreateParticipantCommand(
    string Name,
    string? Email,
    string? Interests) : ICommand<Result<int>>;

