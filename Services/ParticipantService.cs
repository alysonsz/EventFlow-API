using AutoMapper;
using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Repository;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;

namespace EventFlow_API.Services;

public class ParticipantService(IParticipantRepository repository, IEventRepository eventRepository, EventFlowContext context, IMapper mapper) : IParticipantService
{
    public async Task<ParticipantDTO?> GetByIdAsync(int id)
    {
        var entity = await repository.GetParticipantByIdAsync(id);
        return entity is null ? 
            null : mapper.Map<ParticipantDTO>(entity);
    }

    public async Task<List<ParticipantDTO>> GetAllAsync(int eventId)
    {
        var entities = await repository.GetAllParticipantsByEventIdAsync(eventId);
        return mapper.Map<List<ParticipantDTO>>(entities);
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
        return updated is null ?
            null : 
            mapper.Map<ParticipantDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
