namespace EventFlow.Core.Repository.Interfaces;

public interface IParticipantRepository
{
    Task<Participant> PostAsync(Participant participant);
    Task<Participant> UpdateAsync(Participant participant);
    Task<int> DeleteAsync(int id);
    Task<Participant?> GetParticipantByIdAsync(int id);
    Task<PagedResult<Participant>> GetAllPagedParticipantsByEventIdAsync(int eventId, QueryParameters queryParameters);
    Task<List<Participant>> GetAllParticipantsWithEventsAsync();
    Task<int> ParticipantCountAsync();
}
