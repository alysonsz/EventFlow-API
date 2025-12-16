namespace EventFlow.Core.Repository.Interfaces;

public interface ISpeakerRepository
{
    Task<Speaker> PostAsync(Speaker speaker);
    Task<Speaker> UpdateAsync(Speaker speaker);
    Task<int> DeleteAsync(int id);
    Task<Speaker?> GetSpeakerByIdAsync(int id);
    Task<List<Speaker>> GetAllSpeakersAsync();
    Task<PagedResult<Speaker>> GetAllPagedSpeakersAsync(QueryParameters queryParameters);
    Task AddSpeakerEventAsync(SpeakerEvent speakerEvent);
    Task<int> SpeakerCountAsync();
}