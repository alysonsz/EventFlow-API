using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface IParticipantRepository
{
    Task<Participant> PostAsync(Participant participant);
    Task<Participant> UpdateAsync(Participant participant);
    Task<Participant> DeleteAsync(Participant participant);
    Task<Participant?> GetParticipantByIdAsync(int Id);
    Task<List<Participant>> GetAllParticipantsAsync(int Id);
}
