using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;

namespace EventFlow_API.Services;

public class EventService(IEventRepository repository) : IEventService
{
    public async Task<Event?> GetByIdAsync(int id)
        => await repository.GetEventWithDetailsByIdAsync(id);

    public async Task<List<Event>> GetAllAsync()
        => await repository.GetAllEventsAsync();

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

    public async Task<Event?> UpdateAsync(int id, EventCommand command)
    {
        var existing = await repository.GetEventByIdAsync(id);
        if (existing == null) return null;

        existing.Title = command.Title;
        existing.Description = command.Description;
        existing.Date = command.Date;
        existing.Location = command.Location;
        existing.OrganizerId = command.OrganizerId;

        return await repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
