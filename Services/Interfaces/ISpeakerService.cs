using EventFlow_API.Commands;
using EventFlow_API.Models;

namespace EventFlow_API.Services.Interfaces;

public interface ISpeakerService
{
    Task<Speaker?> GetByIdAsync(int id);
    Task<List<Speaker>> GetAllAsync();
    Task<Speaker?> CreateAsync(SpeakerCommand command);
    Task<Speaker?> UpdateAsync(int id, SpeakerCommand command);
    Task<bool> DeleteAsync(int id);
}

