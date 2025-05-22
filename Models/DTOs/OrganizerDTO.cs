namespace EventFlow_API.Models.DTOs;

public class OrganizerDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<EventSummaryDTO> Events { get; set; } = [];
}

