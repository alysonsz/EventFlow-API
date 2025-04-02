using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface IOrganizerRepository
{
    Task<Organizer> PostAsync(Organizer organizer);
    Task<Organizer> UpdateAsync(Organizer organizer);
    Task<Organizer> DeleteAsync(Organizer organizer);
    Task<Organizer?> GetOrganizerByIdAsync(int Id);
    Task<List<Organizer>> GetAllOrganizersAsync(int Id);
}
