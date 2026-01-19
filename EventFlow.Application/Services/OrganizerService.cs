using EventFlow.Core.Models.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventFlow.Application.Services;

public class OrganizerService(IOrganizerRepository repository, IEventRepository eventRepository, 
    IMapper mapper, IDistributedCache cache) : IOrganizerService
{
    public async Task<OrganizerDTO?> GetByIdAsync(int id)
    {
        string cacheKey = $"organizer-{id}";

        var cachedData = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<OrganizerDTO>(cachedData)!;
        }

        var entity = await repository.GetOrganizerByIdAsync(id);

        if (entity == null)
            return null;

        var dto = mapper.Map<OrganizerDTO>(entity);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return dto;
    }

    public async Task<List<OrganizerDTO>> GetAllOrganizersAsync()
    {
        var organizers = await repository.GetAllOrganizersAsync();
        return mapper.Map<List<OrganizerDTO>>(organizers);
    }

    public async Task<PagedResult<OrganizerDTO>> GetAllPagedOrganizersAsync(QueryParameters queryParameters)
    {
        var pagedOrganizers = await repository.GetAllPagedOrganizersAsync(queryParameters);

        var organizerDtos = mapper.Map<List<OrganizerDTO>>(pagedOrganizers.Items);

        return new PagedResult<OrganizerDTO>(
            organizerDtos,
            pagedOrganizers.PageNumber,
            pagedOrganizers.PageSize,
            pagedOrganizers.TotalCount
        );
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

        await cache.RemoveAsync($"organizer-{id}");

        return updated is null ? null : mapper.Map<OrganizerDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);

        if (deleted > 0)
        {
            await cache.RemoveAsync($"organizer-{id}");
        }
        
        return deleted > 0;
    }
}
