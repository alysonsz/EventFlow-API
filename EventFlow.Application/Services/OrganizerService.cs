using EventFlow.Core.Models.DTOs;

namespace EventFlow.Application.Services;

public class OrganizerService(IOrganizerRepository repository, IEventRepository eventRepository, IMapper mapper) : IOrganizerService
{
    public async Task<OrganizerDTO?> GetByIdAsync(int id)
    {
        var entity = await repository.GetOrganizerByIdAsync(id);
        return entity is null ?
            null :
            mapper.Map<OrganizerDTO>(entity);
    }

    public async Task<List<OrganizerDTO>> GetAllAsync()
    {
        var entities = await repository.GetAllOrganizersAsync();
        return mapper.Map<List<OrganizerDTO>>(entities);
    }

    public async Task<Organizer?> CreateAsync(OrganizerCommand command)
    {
        var organizer = new Organizer
        {
            Name = command.Name,
            Email = command.Email
        };

        return await repository.PostAsync(organizer);
    }

    public async Task<bool> RegisterToEventAsync(int eventId, int organizerId)
    {
        var organizer = await repository.GetOrganizerByIdAsync(organizerId);
        var evento = await eventRepository.GetEventByIdAsync(eventId);

        if (organizer == null || evento == null)
            return false;

        evento.OrganizerId = organizerId;
        evento.Organizer = organizer;

        await eventRepository.UpdateAsync(evento);
        return true;
    }

    public async Task<OrganizerDTO?> UpdateAsync(int id, OrganizerCommand command)
    {
        var existing = await repository.GetOrganizerByIdAsync(id);
        if (existing == null)
            return null;

        existing.Name = command.Name;
        existing.Email = command.Email;

        var updated = await repository.UpdateAsync(existing);
        return updated is null ?
            null :
            mapper.Map<OrganizerDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
