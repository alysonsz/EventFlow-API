using Microsoft.EntityFrameworkCore;

namespace EventFlow.Infrastructure.Repository;

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
            .Include(p => p.Events!)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PagedResult<Participant>> GetAllParticipantsByEventIdAsync(int eventId, QueryParameters queryParameters)
    {
        var query = context.Participant
            .Where(p => p.Events!.Any(e => e.Id == eventId))
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.Filter))
        {
            var filter = queryParameters.Filter.ToLowerInvariant();
            query = query.Where(p =>
                p.Name.ToLowerInvariant().Contains(filter) ||
                p.Email.ToLowerInvariant().Contains(filter)
            );
        }

        query = queryParameters.SortBy?.ToLowerInvariant() switch
        {
            "name_desc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name) 
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Participant>(items, queryParameters.PageNumber, queryParameters.PageSize, totalCount);
    }

    public async Task<int> ParticipantCountAsync()
    {
        return await context.Participant.CountAsync();
    }
}
