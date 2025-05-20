using EventFlow_API.Commands;
using EventFlow_API.Models;

namespace EventFlow_API.Services.Interfaces;

public interface IParticipantService
{
    Task<Participant?> GetByIdAsync(int id);
    Task<List<Participant>> GetAllAsync(int eventId);
    Task<Participant?> CreateAsync(ParticipantCommand command);
    Task<Participant?> UpdateAsync(int id, ParticipantCommand command);
    Task<bool> DeleteAsync(int id);
}
