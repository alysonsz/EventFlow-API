using AutoMapper;
using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;

namespace EventFlow_API.Services;

public class EventService(IEventRepository repository, IMapper mapper) : IEventService
{
    public async Task<EventDTO?> GetByIdAsync(int id)
    {
        var entity = await repository.GetEventWithDetailsByIdAsync(id);

        return entity is null ?
            null :
            mapper.Map<EventDTO>(entity);
    }

    public async Task<List<EventDTO>> GetAllAsync()
    {
        var events = await repository.GetAllEventsAsync();
        return mapper.Map<List<EventDTO>>(events);
    }

    public async Task<Event?> CreateAsync(EventCommand command)
    {
        var newEvent = new Event
        {
            Title = command.Title,
            Description = command.Description,
            Date = command.Date,
            Location = command.Location,
            OrganizerId = command.OrganizerId
        };

        return await repository.PostAsync(newEvent);
    }

    public async Task<EventDTO?> UpdateAsync(int id, EventCommand command)
    {
        var existing = await repository.GetEventByIdAsync(id);
        if (existing == null) return null;

        existing.Title = command.Title;
        existing.Description = command.Description;
        existing.Date = command.Date;
        existing.Location = command.Location;
        existing.OrganizerId = command.OrganizerId;

        var updated = await repository.UpdateAsync(existing);
        return updated is null ?
            null :
            mapper.Map<EventDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
