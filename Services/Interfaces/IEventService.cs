using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;

namespace EventFlow_API.Services.Interfaces;

public interface IEventService
{
    Task<EventDTO?> GetByIdAsync(int id);
    Task<List<EventDTO>> GetAllAsync();
    Task<Event?> CreateAsync(EventCommand command);
    Task<EventDTO?> UpdateAsync(int id, EventCommand command);
    Task<bool> DeleteAsync(int id);
}
