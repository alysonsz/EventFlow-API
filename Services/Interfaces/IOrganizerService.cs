using EventFlow_API.Commands;
using EventFlow_API.Models;

namespace EventFlow_API.Services.Interfaces;

public interface IOrganizerService
{
    Task<Organizer?> GetByIdAsync(int id);
    Task<List<Organizer>> GetAllAsync();
    Task<Organizer?> CreateAsync(OrganizerCommand command);
    Task<Organizer?> UpdateAsync(int id, OrganizerCommand command);
    Task<bool> DeleteAsync(int id);
}
