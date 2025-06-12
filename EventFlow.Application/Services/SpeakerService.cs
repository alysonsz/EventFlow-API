using EventFlow.Core.Models.DTOs;

namespace EventFlow.Application.Services;

public class SpeakerService(ISpeakerRepository repository, IEventRepository eventRepository, IMapper mapper) : ISpeakerService
{
    public async Task<SpeakerDTO?> GetByIdAsync(int id)
    {
        var entity = await repository.GetSpeakerByIdAsync(id);
        return entity is null ?
            null :
            mapper.Map<SpeakerDTO>(entity);
    }


    public async Task<PagedResult<SpeakerDTO>> GetAllSpeakersAsync(QueryParameters queryParameters)
    {
        var pagedSpeakers = await repository.GetAllSpeakersAsync(queryParameters);

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
        return updated is null ?
           null :
           mapper.Map<SpeakerDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
