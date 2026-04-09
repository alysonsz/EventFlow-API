using System.Text.Json.Serialization;

namespace EventFlow.Application.Commands;

public class SpeakerCommand
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Biography { get; set; }

    [JsonIgnore]
    public int EventId { get; set; }
}
