using EventNucleus.Core.Primitives;

namespace EventNucleus.Application.Features.Organizers.Commands.DeleteOrganizer;

public record DeleteOrganizerCommand(int Id) : ICommand<Result>;

