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

    public async Task<int> DeleteAsync(int id)
    {
        var speakerToDelete = await context.Speaker.FindAsync(id);
        if (speakerToDelete != null)
        {
            context.Speaker.Remove(speakerToDelete);
            await context.SaveChangesAsync();
        }
        return id;
    }

    public async Task<Speaker?> GetSpeakerByIdAsync(int id)
    {
        return await context.Speaker
            .Include(s => s.SpeakerEvents)
                .ThenInclude(se => se.Event)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Speaker>> GetAllSpeakersAsync()
    {
        return await context.Speaker
            .Include(s => s.SpeakerEvents)
                .ThenInclude(se => se.Event)
            .ToListAsync() ?? [];
    }
}
