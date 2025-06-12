namespace EventFlow.Core.Repository.Interfaces;

public interface IEventRepository
{
    Task<Event> PostAsync(Event @event);
    Task<Event> UpdateAsync(Event @event);
    Task<int> DeleteAsync(int id);
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event?> GetEventWithDetailsByIdAsync(int id);
    Task<PagedResult<Event>> GetAllEventsAsync(QueryParameters queryParameters);
}
