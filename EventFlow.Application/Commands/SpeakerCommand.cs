using System.Text.Json.Serialization;

namespace EventFlow.Application.Commands;

public class SpeakerCommand
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Biography { get; set; }

    [JsonIgnore]
    public int EventId { get; set; }
}
