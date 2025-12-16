namespace EventFlow.Core.Services.Interfaces;

public interface IEventService
{
    Task<EventDTO?> GetByIdAsync(int id);
    Task<List<EventDTO>> GetAllEventsAsync();
    Task<PagedResult<EventDTO>> GetAllPagedEventsAsync(QueryParameters queryParameters);
    Task<Event?> CreateAsync(EventCommand command);
    Task<EventDTO?> UpdateAsync(int id, EventCommand command);
    Task<bool> DeleteAsync(int id);
}
