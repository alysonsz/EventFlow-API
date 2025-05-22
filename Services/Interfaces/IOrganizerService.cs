using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;

namespace EventFlow_API.Services.Interfaces;

public interface IOrganizerService
{
    Task<OrganizerDTO?> GetByIdAsync(int id);
    Task<List<OrganizerDTO>> GetAllAsync();
    Task<Organizer?> CreateAsync(OrganizerCommand command);
    Task<bool> RegisterToEventAsync(int eventId, int organizerId);
    Task<OrganizerDTO?> UpdateAsync(int id, OrganizerCommand command);
    Task<bool> DeleteAsync(int id);
}
