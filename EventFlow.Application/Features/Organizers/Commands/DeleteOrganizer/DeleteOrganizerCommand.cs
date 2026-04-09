using EventFlow.Core.Primitives;

namespace EventFlow.Application.Features.Organizers.Commands.DeleteOrganizer;

public record DeleteOrganizerCommand(int Id) : ICommand<Result>;
