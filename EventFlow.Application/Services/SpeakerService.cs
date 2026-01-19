using EventFlow.Core.Models.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventFlow.Application.Services;

public class SpeakerService(ISpeakerRepository repository, IEventRepository eventRepository,
    IMapper mapper, IDistributedCache cache) : ISpeakerService
{
    public async Task<SpeakerDTO?> GetByIdAsync(int id)
    {
        string cacheKey = $"speaker-{id}";

        var cachedData = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<SpeakerDTO>(cachedData)!;
        }

        var entity = await repository.GetSpeakerByIdAsync(id);

        if (entity == null)
            return null;

        var dto = mapper.Map<SpeakerDTO>(entity);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return dto;
    }

    public async Task<List<SpeakerDTO>> GetAllSpeakersAsync()
    {
        var speakers = await repository.GetAllSpeakersAsync();
        return mapper.Map<List<SpeakerDTO>>(speakers);
    }

    public async Task<PagedResult<SpeakerDTO>> GetAllPagedSpeakersAsync(QueryParameters queryParameters)
    {
        var pagedSpeakers = await repository.GetAllPagedSpeakersAsync(queryParameters);

        var speakerDtos = mapper.Map<List<SpeakerDTO>>(pagedSpeakers.Items);

        return new PagedResult<SpeakerDTO>(
            speakerDtos,
            pagedSpeakers.PageNumber,
            pagedSpeakers.PageSize,
            pagedSpeakers.TotalCount
        );
    }

    public async Task<Speaker?> CreateAsync(SpeakerCommand command)
    {
        var speaker = new Speaker
        {
            Name = command.Name,
            Email = command.Email,
            Biography = command.Biography
        };

        return await repository.PostAsync(speaker);
    }
    public async Task<bool> RegisterToEventAsync(int eventId, int speakerId)
    {
        var speaker = await repository.GetSpeakerByIdAsync(speakerId);
        var evento = await eventRepository.GetEventByIdAsync(eventId);

        if (speaker == null || evento == null)
            return false;

        var alreadyLinked = speaker.SpeakerEvents.Any(se => se.EventId == eventId);
        if (alreadyLinked)
            return true;
        var speakerEvent = new SpeakerEvent
        {
            SpeakerId = speakerId,
            EventId = eventId,
            RegisteredAt = DateTime.UtcNow
        };

        await repository.AddSpeakerEventAsync(speakerEvent);

        await cache.RemoveAsync($"speaker-{speakerId}");

        return true;
    }

    public async Task<SpeakerDTO?> UpdateAsync(int id, SpeakerCommand command)
    {
        var existing = await repository.GetSpeakerByIdAsync(id);
        if (existing == null) return null;

        existing.Name = command.Name;
        existing.Email = command.Email;
        existing.Biography = command.Biography;

        var updated = await repository.UpdateAsync(existing);

        await cache.RemoveAsync($"speaker-{id}");

        return updated is null ? null : mapper.Map<SpeakerDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);

        if (deleted > 0)
        {
            await cache.RemoveAsync($"speaker-{id}");
        }

        return deleted > 0;
    }
}
