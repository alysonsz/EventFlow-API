namespace EventFlow.Core.Services.Interfaces;

public interface IParticipantService
{
    Task<ParticipantDTO?> GetByIdAsync(int id);
    Task<PagedResult<ParticipantDTO>> GetAllPagedParticipantsByEventIdAsync(int eventId, QueryParameters queryParameters);
    Task<Participant?> CreateAsync(ParticipantCommand command);
    Task<bool> RegisterToEventAsync(int eventId, int participantId);
    Task<ParticipantDTO?> UpdateAsync(int id, ParticipantCommand command);
    Task<List<ParticipantDTO>> GetAllParticipantsWithEventsAsync();
    Task<bool> DeleteAsync(int id);
}
