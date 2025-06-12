namespace EventFlow.Core.Services.Interfaces;

public interface IOrganizerService
{
    Task<OrganizerDTO?> GetByIdAsync(int id);
    Task<PagedResult<OrganizerDTO>> GetAllOrganizersAsync(QueryParameters queryParameters);
    Task<Organizer?> CreateAsync(OrganizerCommand command);
    Task<bool> RegisterToEventAsync(int eventId, int organizerId);
    Task<OrganizerDTO?> UpdateAsync(int id, OrganizerCommand command);
    Task<bool> DeleteAsync(int id);
}
