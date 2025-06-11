namespace EventFlow.Core.Models.DTOs;

public class SpeakerDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<EventSummaryDTO> Events { get; set; } = [];
}
