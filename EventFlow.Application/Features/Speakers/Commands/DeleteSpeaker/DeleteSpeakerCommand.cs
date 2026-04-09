using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Speakers.Commands.DeleteSpeaker;

public record DeleteSpeakerCommand(int Id) : ICommand<Result>;
