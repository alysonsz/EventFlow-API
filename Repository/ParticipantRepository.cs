using EventFlow_API.Data;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;

namespace EventFlow_API.Repository;

public class ParticipantRepository(EventFlowContext context) : IParticipantRepository
{
    public async Task<Participant> PostAsync(Participant participant)
    {
        await context.Participant.AddAsync(participant);
        await context.SaveChangesAsync();
        return participant;
    }

    public async Task<Participant> UpdateAsync(Participant participant)
    {
        context.Participant.Update(participant);
        await context.SaveChangesAsync();
        return participant;
    }

    public async Task<Participant> DeleteAsync(Participant participant)
    {
        context.Participant.Remove(participant);
        await context.SaveChangesAsync();
        return participant;
    }

    public async Task<Participant?> GetParticipantByIdAsync(int id)
    {
        return await context.Participant
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Participant>> GetAllParticipantsAsync(int id)
    {
        return await context.Participant
            .Where(p => p.Id == id)
            .ToListAsync() ?? new List<Participant>();
    }
}
