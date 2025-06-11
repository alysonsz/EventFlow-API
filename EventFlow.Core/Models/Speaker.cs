namespace EventFlow.Core.Models;

public class Speaker
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Biography { get; set; }
    public string Email { get; set; }
    public ICollection<SpeakerEvent> SpeakerEvents { get; set; } = [];

}
