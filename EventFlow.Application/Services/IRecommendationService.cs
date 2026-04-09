namespace EventFlow.Application.Services;

public interface IRecommendationService
{
    Task<IEnumerable<EventDTO>> GetRecommendedEventsAsync(int participantId);
    Task<IEnumerable<ParticipantDTO>> GetRecommendedConnectionsAsync(int participantId);
}
