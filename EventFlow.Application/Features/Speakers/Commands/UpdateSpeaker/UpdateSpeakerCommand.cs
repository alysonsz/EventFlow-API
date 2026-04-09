using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Speakers.Commands.UpdateSpeaker;

public record UpdateSpeakerCommand(
    int Id,
    string Name,
    string? Email,
    string? Biography,
    string? ExpertiseArea) : ICommand<Result>;
