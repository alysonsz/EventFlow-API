using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;

namespace EventFlow_API.Repository;

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
            .Include(e => e.Speakers)
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await context.Event
            .ToListAsync() ?? [];
    }
}
