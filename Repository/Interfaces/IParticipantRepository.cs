using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface IParticipantRepository
{
    Task<Participant> PostAsync(Participant participant);
    Task<Participant> UpdateAsync(Participant participant);
    Task<int> DeleteAsync(int id);
    Task<Participant?> GetParticipantByIdAsync(int id);
    Task<List<Participant>> GetAllParticipantsByEventIdAsync(int eventId);
}
