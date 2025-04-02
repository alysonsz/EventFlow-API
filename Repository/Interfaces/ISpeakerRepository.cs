using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface ISpeakerRepository
{
    Task<Speaker> PostAsync(Speaker speaker);
    Task<Speaker> UpdateAsync(Speaker speaker);
    Task<Speaker> DeleteAsync(Speaker speaker);
    Task<Speaker?> GetSpeakerByIdAsync(int id);
    Task<List<Speaker>> GetAllSpeakersAsync(int id);
}