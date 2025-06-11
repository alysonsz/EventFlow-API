namespace EventFlow.Core.Services.Interfaces;

public interface ISpeakerService
{
    Task<SpeakerDTO?> GetByIdAsync(int id);
    Task<List<SpeakerDTO>> GetAllAsync();
    Task<Speaker?> CreateAsync(SpeakerCommand command);
    Task<bool> RegisterToEventAsync(int eventId, int speakerId);
    Task<SpeakerDTO?> UpdateAsync(int id, SpeakerCommand command);
    Task<bool> DeleteAsync(int id);
}

