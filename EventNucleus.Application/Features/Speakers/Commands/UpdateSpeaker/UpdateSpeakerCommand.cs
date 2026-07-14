using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Speakers.Commands.UpdateSpeaker;

public record UpdateSpeakerCommand(
    int Id,
    string Name,
    string? Email,
    string? Biography,
    string? ExpertiseArea) : ICommand<Result>;

