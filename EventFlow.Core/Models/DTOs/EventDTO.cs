namespace EventFlow.Core.Models.DTOs;

public class EventDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Category { get; set; }
    public OrganizerDTO? Organizer { get; set; }
    public List<SpeakerDTO> Speakers { get; set; } = [];
    public List<ParticipantDTO> Participants { get; set; } = [];
}

