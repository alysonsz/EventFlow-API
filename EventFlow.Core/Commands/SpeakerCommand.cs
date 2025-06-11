namespace EventFlow.Core.Commands;

public class SpeakerCommand
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Biography { get; set; }
    public int EventId { get; set; }
}
