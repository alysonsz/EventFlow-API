using EventFlow_API.Data;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;

namespace EventFlow_API.Repository;

public class SpeakerRepository(EventFlowContext context) : ISpeakerRepository
{
    public async Task<Speaker> PostAsync(Speaker speaker)
    {
        await context.Speaker.AddAsync(speaker);
        await context.SaveChangesAsync();
        return speaker;
    }

    public async Task<Speaker> UpdateAsync(Speaker speaker)
    {
        context.Speaker.Update(speaker);
        await context.SaveChangesAsync();
        return speaker;
    }

    public async Task<Speaker> DeleteAsync(Speaker speaker)
    {
        context.Speaker.Remove(speaker);
        await context.SaveChangesAsync();
        return speaker;
    }

    public async Task<Speaker?> GetSpeakerByIdAsync(int id)
    {
        return await context.Speaker
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Speaker>> GetAllSpeakersAsync(int id)
    {
        return await context.Speaker
            .Where(s => s.Id == id)
            .ToListAsync() ?? new List<Speaker>();
    }
}
