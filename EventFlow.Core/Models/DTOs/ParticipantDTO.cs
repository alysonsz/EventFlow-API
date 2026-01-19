namespace EventFlow.Core.Models.DTOs;

public class ParticipantDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Interests { get; set; }
}

