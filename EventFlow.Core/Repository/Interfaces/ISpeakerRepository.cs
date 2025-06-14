namespace EventFlow.Core.Repository.Interfaces;

public interface ISpeakerRepository
{
    Task<Speaker> PostAsync(Speaker speaker);
    Task<Speaker> UpdateAsync(Speaker speaker);
    Task<int> DeleteAsync(int id);
    Task<Speaker?> GetSpeakerByIdAsync(int id);
    Task<PagedResult<Speaker>> GetAllSpeakersAsync(QueryParameters queryParameters);
    Task AddSpeakerEventAsync(SpeakerEvent speakerEvent);
    Task<int> SpeakerCountAsync();
}