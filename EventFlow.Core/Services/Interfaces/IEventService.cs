namespace EventFlow.Core.Services.Interfaces;

public interface IEventService
{
    Task<EventDTO?> GetByIdAsync(int id);
    Task<PagedResult<EventDTO>> GetAllEventsAsync(QueryParameters queryParameters);
    Task<Event?> CreateAsync(EventCommand command);
    Task<EventDTO?> UpdateAsync(int id, EventCommand command);
    Task<bool> DeleteAsync(int id);
}
