using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Speakers.Commands.DeleteSpeaker;

public record DeleteSpeakerCommand(int Id) : ICommand<Result>;

