using EventFlow.Core.Models.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventFlow.Application.Services;

public class ParticipantService(IParticipantRepository repository, IEventRepository eventRepository, 
    IMapper mapper, IDistributedCache cache) : IParticipantService
{
    public async Task<ParticipantDTO?> GetByIdAsync(int id)
    {
        string cacheKey = $"participant-{id}";

        var cachedData = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<ParticipantDTO>(cachedData)!;
        }

        var entity = await repository.GetParticipantByIdAsync(id);

        if (entity == null)
            return null;

        var dto = mapper.Map<ParticipantDTO>(entity);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return dto;
    }

    public async Task<PagedResult<ParticipantDTO>> GetAllPagedParticipantsByEventIdAsync(int eventId, QueryParameters queryParameters)
    {
        var pagedParticipants = await repository.GetAllPagedParticipantsByEventIdAsync(eventId, queryParameters);

        var participantDtos = mapper.Map<List<ParticipantDTO>>(pagedParticipants.Items);

        return new PagedResult<ParticipantDTO>(
            participantDtos,
            pagedParticipants.PageNumber,
            pagedParticipants.PageSize,
            pagedParticipants.TotalCount
        );
    }

    public async Task<Participant?> CreateAsync(ParticipantCommand command)
    {
        var participant = new Participant
        {
            Name = command.Name,
            Email = command.Email
        };

        return await repository.PostAsync(participant);
    }

    public async Task<bool> RegisterToEventAsync(int eventId, int participantId)
    {
        var participant = await repository.GetParticipantByIdAsync(participantId);
        var evento = await eventRepository.GetEventByIdAsync(eventId);

        if (evento == null || participant == null)
            return false;

        participant.Events!.Add(evento);
        await repository.UpdateAsync(participant);

        await cache.RemoveAsync($"participant-{participantId}");

        return true;
    }

    public async Task<ParticipantDTO?> UpdateAsync(int id, ParticipantCommand command)
    {
        var existing = await repository.GetParticipantByIdAsync(id);

        if (existing == null)
            return null;

        existing.Name = command.Name;
        existing.Email = command.Email;

        var updated = await repository.UpdateAsync(existing);

        await cache.RemoveAsync($"participant-{id}");

        return updated is null ? null : mapper.Map<ParticipantDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        
        if (deleted > 0)
        {
            await cache.RemoveAsync($"participant-{id}");
        }

        return deleted > 0;
    }

    public async Task<List<ParticipantDTO>> GetAllParticipantsWithEventsAsync()
    {
        var allParticipants = await repository.GetAllParticipantsWithEventsAsync();
        return mapper.Map<List<ParticipantDTO>>(allParticipants);
    }
}
