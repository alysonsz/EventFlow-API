namespace EventFlow_API.Commands;

public class SpeakerCommand
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Biography { get; set; }
    public int EventId { get; set; }
}
