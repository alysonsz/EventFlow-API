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
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Organizer>> GetAllOrganizersAsync()
    {
        return await context.Organizer
            .ToListAsync() ?? [];
    }
}
