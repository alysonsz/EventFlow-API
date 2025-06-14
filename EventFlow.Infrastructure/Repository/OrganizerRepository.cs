using Microsoft.EntityFrameworkCore;

namespace EventFlow.Infrastructure.Repository;

public class OrganizerRepository(EventFlowContext context) : IOrganizerRepository
{
    public async Task<Organizer> PostAsync(Organizer organizer)
    {
        await context.Organizer.AddAsync(organizer);
        await context.SaveChangesAsync();
        return organizer;
    }

    public async Task<Organizer> UpdateAsync(Organizer organizer)
    {
        context.Organizer.Update(organizer);
        await context.SaveChangesAsync();
        return organizer;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var organizerToDelete = await context.Organizer.FindAsync(id);
        if (organizerToDelete != null)
        {
            context.Organizer.Remove(organizerToDelete);
            await context.SaveChangesAsync();
        }
        return id;
    }

    public async Task<Organizer?> GetOrganizerByIdAsync(int id)
    {
        return await context.Organizer
            .Include(o => o.Events!)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<PagedResult<Organizer>> GetAllOrganizersAsync(QueryParameters queryParameters)
    {
        var query = context.Organizer
            .Include(o => o.Events)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.Filter))
        {
            var filter = queryParameters.Filter.ToLowerInvariant();
            query = query.Where(o =>
                o.Name.ToLowerInvariant().Contains(filter) ||
                o.Email.ToLowerInvariant().Contains(filter)
            );
        }

        query = queryParameters.SortBy?.ToLowerInvariant() switch
        {
            "name_desc" => query.OrderByDescending(o => o.Name),
            _ => query.OrderBy(o => o.Name) 
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResult<Organizer>(items, queryParameters.PageNumber, queryParameters.PageSize, totalCount);
    }

    public async Task<int> OrganizerCountAsync()
    {
        return await context.Organizer.CountAsync();
    }
}
