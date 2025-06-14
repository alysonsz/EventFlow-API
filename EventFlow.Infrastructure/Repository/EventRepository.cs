using EventFlow.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Infrastructure.Repository;

public class EventRepository(EventFlowContext context) : IEventRepository
{

    public async Task<Event> PostAsync(Event @event)
    {
        await context.Event.AddAsync(@event);
        await context.SaveChangesAsync();
        return @event;
    }

    public async Task<Event> UpdateAsync(Event @event)
    {
        context.Event.Update(@event);
        await context.SaveChangesAsync();
        return @event;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var eventToDelete = await context.Event.FindAsync(id);
        if (eventToDelete != null)
        {
            context.Event.Remove(eventToDelete);
            await context.SaveChangesAsync();
        }
        return id;
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await context.Event
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetEventWithDetailsByIdAsync(int id)
    {
        return await context.Event
            .Include(e => e.Organizer)
            .Include(e => e.SpeakerEvents)
                .ThenInclude(se => se.Speaker)
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<PagedResult<Event>> GetAllEventsAsync(QueryParameters queryParameters)
    {
        var query = context.Event
            .Include(e => e.Organizer)
            .Include(e => e.SpeakerEvents)
                .ThenInclude(se => se.Speaker)
            .Include(e => e.Participants)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.Filter))
        {
            var filter = queryParameters.Filter.ToLowerInvariant();
            query = query.Where(e =>
                e.Title.ToLowerInvariant().Contains(filter) ||
                e.Location.ToLowerInvariant().Contains(filter)
            );
        }

        query = queryParameters.SortBy?.ToLowerInvariant() switch
        {
            "date_desc" => query.OrderByDescending(e => e.Date),
            "title" => query.OrderBy(e => e.Title),
            "title_desc" => query.OrderByDescending(e => e.Title),
            _ => query.OrderBy(e => e.Date)
        };

        var totalCount = await query.CountAsync();
        var items = await query
        .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
        .Take(queryParameters.PageSize)
        .ToListAsync();

        return new PagedResult<Event>(items, queryParameters.PageNumber, queryParameters.PageSize, totalCount);
    }

    public async Task<int> EventCountAsync()
    {
        return await context.Event.CountAsync();
    }
}
