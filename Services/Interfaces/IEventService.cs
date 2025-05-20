using EventFlow_API.Commands;
using EventFlow_API.Models;

namespace EventFlow_API.Services.Interfaces;

public interface IEventService
{
    Task<Event?> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task<Event?> CreateAsync(EventCommand command);
    Task<Event?> UpdateAsync(int id, EventCommand command);
    Task<bool> DeleteAsync(int id);
}
