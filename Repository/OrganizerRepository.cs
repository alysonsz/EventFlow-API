using EventFlow_API.Data;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;

namespace EventFlow_API.Repository;

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

    public async Task<Organizer> DeleteAsync(Organizer organizer)
    {
        context.Organizer.Remove(organizer);
        await context.SaveChangesAsync();
        return organizer;
    }

    public async Task<Organizer?> GetOrganizerByIdAsync(int id)
    {
        return await context.Organizer
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Organizer>> GetAllOrganizersAsync(int id)
    {
        return await context.Organizer
            .Where(o => o.Id == id)
            .ToListAsync() ?? new List<Organizer>();
    }
}
