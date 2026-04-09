namespace EventFlow.Application.Services;

public interface IRecommendationService
{
    Task<IEnumerable<EventDTO>> GetRecommendedEventsAsync(int participantId);
    Task<IEnumerable<object>> GetRecommendedConnectionsAsync(int participantId);
}
