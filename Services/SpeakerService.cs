using EventFlow_API.Services.Interfaces;
using EventFlow_API.Models;
using EventFlow_API.Commands;
using EventFlow_API.Repository.Interfaces;

namespace EventFlow_API.Services;

public class SpeakerService(ISpeakerRepository repository) : ISpeakerService
{
    public async Task<Speaker?> GetByIdAsync(int id)
        => await repository.GetSpeakerByIdAsync(id);

    public async Task<List<Speaker>> GetAllAsync()
        => await repository.GetAllSpeakersAsync();

    public async Task<Speaker?> CreateAsync(SpeakerCommand command)
    {
        var speaker = new Speaker
        {
            Name = command.Name,
            Email = command.Email,
            Biography = command.Biography,
            EventId = command.EventId
        };

        return await repository.PostAsync(speaker);
    }

    public async Task<Speaker?> UpdateAsync(int id, SpeakerCommand command)
    {
        var existing = await repository.GetSpeakerByIdAsync(id);
        if (existing == null) return null;

        existing.Name = command.Name;
        existing.Email = command.Email;
        existing.Biography = command.Biography;
        existing.EventId = command.EventId;

        return await repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await repository.DeleteAsync(id);
        return deleted > 0;
    }
}
