using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Participants.Commands.UpdateParticipant;

public record UpdateParticipantCommand(
    int Id,
    string Name,
    string? Email,
    string? Interests) : ICommand<Result>;

