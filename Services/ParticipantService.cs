using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using EventFlow_API.Services.Interfaces;

namespace EventFlow_API.Services;

public class ParticipantService(IParticipantRepository repository) : IParticipantService
{
    public async Task<Participant?> GetByIdAsync(int id)
        => await repository.GetParticipantByIdAsync(id);

    public async Task<List<Participant>> GetAllAsync(int eventId)
        => await repository.GetAllParticipantsByEventIdAsync(eventId);

    public async Task<Participant?> CreateAsync(ParticipantCommand command)
    {
        var participant = new Participant
        {
            Name = command.Name,
            Email = command.Email
        };

        return await repository.PostAsync(participant);
    }

    public async Task<Participant?> UpdateAsync(int id, ParticipantCommand command)
    {
        var existing = await repository.GetParticipantByIdAsync(id);
        if (existing == null) return null;

        existing.Name = command.Name;
        existing.Email = command.Email;

        return await repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
