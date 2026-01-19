using EventFlow.Core.Models.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace EventFlow.Application.Services;

public class EventService(IEventRepository repository, IMapper mapper, IDistributedCache cache) : IEventService
{
    public async Task<EventDTO?> GetByIdAsync(int id)
    {
        string cacheKey = $"event-{id}";

        var cachedData = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<EventDTO>(cachedData)!;
        }

        var entity = await repository.GetEventWithDetailsByIdAsync(id);

        if (entity == null)
            return null;

        var dto = mapper.Map<EventDTO>(entity);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return dto;
    }

    public async Task<List<EventDTO>> GetAllEventsAsync()
    {
        var events = await repository.GetAllEventsAsync();
        return mapper.Map<List<EventDTO>>(events);
    }

    public async Task<PagedResult<EventDTO>> GetAllPagedEventsAsync(QueryParameters queryParameters)
    {
        var pagedEvents = await repository.GetAllPagedEventsAsync(queryParameters);

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

        await cache.RemoveAsync($"event-{id}");

        return updated is null ? null : mapper.Map<EventDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);

        if (deleted > 0) 
        {
            await cache.RemoveAsync($"event-{id}"); 
        }

        return deleted > 0;
    }
}
