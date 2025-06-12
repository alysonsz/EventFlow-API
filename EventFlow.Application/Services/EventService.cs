using EventFlow.Core.Models.DTOs;

namespace EventFlow.Application.Services;

public class EventService(IEventRepository repository, IMapper mapper) : IEventService
{
    public async Task<EventDTO?> GetByIdAsync(int id)
    {
        var entity = await repository.GetEventWithDetailsByIdAsync(id);

        return entity is null ?
            null :
            mapper.Map<EventDTO>(entity);
    }

    public async Task<PagedResult<EventDTO>> GetAllEventsAsync(QueryParameters queryParameters)
    {
        var pagedEvents = await repository.GetAllEventsAsync(queryParameters);

        var eventDtos = mapper.Map<List<EventDTO>>(pagedEvents.Items);

        return new PagedResult<EventDTO>(
            eventDtos,
            pagedEvents.PageNumber,
            pagedEvents.PageSize,
            pagedEvents.TotalCount
        );
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

        var createdEvent = await repository.PostAsync(newEvent);
        if (createdEvent == null)
            return null;

        var eventWithDetails = await repository.GetEventWithDetailsByIdAsync(createdEvent.Id);
        return eventWithDetails;
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
