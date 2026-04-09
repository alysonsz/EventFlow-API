using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Speakers.Commands.CreateSpeaker;

public record CreateSpeakerCommand(
    string Name,
    string? Email,
    string? Biography,
    string? ExpertiseArea) : ICommand<Result<int>>;
