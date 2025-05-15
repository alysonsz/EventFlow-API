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

    public async Task<int> DeleteAsync(int id)
    {
        var participantToDelete = await context.Participant.FindAsync(id);
        if (participantToDelete != null)
        {
            context.Participant.Remove(participantToDelete);
            await context.SaveChangesAsync();
        }
        return id;
    }

    public async Task<Participant?> GetParticipantByIdAsync(int id)
    {
        return await context.Participant
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Participant>> GetAllParticipantsByEventIdAsync(int eventId)
    {
        return await context.Participant
        .Where(p => p.Events!.Any(e => e.Id == eventId))
        .ToListAsync() ?? [];
    }
}
