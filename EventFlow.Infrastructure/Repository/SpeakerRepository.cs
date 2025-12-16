namespace EventFlow.Infrastructure.Repository;

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
            .ToListAsync();
    }

    public async Task<PagedResult<Speaker>> GetAllPagedSpeakersAsync(QueryParameters queryParameters)
    {
        var query = context.Speaker
            .Include(s => s.SpeakerEvents)
                .ThenInclude(se => se.Event)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.Filter))
        {
            var filter = queryParameters.Filter.ToLowerInvariant();
            query = query.Where(s =>
                s.Name.ToLowerInvariant().Contains(filter) ||
                s.Email.ToLowerInvariant().Contains(filter)
            );
        }

        query = queryParameters.SortBy?.ToLowerInvariant() switch
        {
            "name_desc" => query.OrderByDescending(s => s.Name),
            _ => query.OrderBy(s => s.Name)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Speaker>(items, queryParameters.PageNumber, queryParameters.PageSize, totalCount);
    }

    public async Task AddSpeakerEventAsync(SpeakerEvent speakerEvent)
    {
        await context.SpeakerEvents.AddAsync(speakerEvent);
        await context.SaveChangesAsync();
    }

    public async Task<int> SpeakerCountAsync()
    {
        return await context.Speaker.CountAsync();
    }
}
