namespace EventFlow.Core.Services.Interfaces;

public interface IParticipantService
{
    Task<ParticipantDTO?> GetByIdAsync(int id);
    Task<List<ParticipantDTO>> GetAllAsync(int eventId);
    Task<Participant?> CreateAsync(ParticipantCommand command);
    Task<bool> RegisterToEventAsync(int eventId, int participantId);
    Task<ParticipantDTO?> UpdateAsync(int id, ParticipantCommand command);
    Task<bool> DeleteAsync(int id);
}
