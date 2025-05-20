using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;

namespace EventFlow_API.Services;

public class OrganizerService(IOrganizerRepository repository) : IOrganizerService
{
    public async Task<Organizer?> GetByIdAsync(int id)
        => await repository.GetOrganizerByIdAsync(id);

    public async Task<List<Organizer>> GetAllAsync()
        => await repository.GetAllOrganizersAsync();

    public async Task<Organizer?> CreateAsync(OrganizerCommand command)
    {
        var organizer = new Organizer
        {
            Name = command.Name,
            Email = command.Email
        };

        return await repository.PostAsync(organizer);
    }

    public async Task<Organizer?> UpdateAsync(int id, OrganizerCommand command)
    {
        var existing = await repository.GetOrganizerByIdAsync(id);
        if (existing == null) return null;

        existing.Name = command.Name;
        existing.Email = command.Email;

        return await repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
