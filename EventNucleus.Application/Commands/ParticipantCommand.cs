namespace EventNucleus.Application.Commands;

public class ParticipantCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Interests { get; set; }
}

