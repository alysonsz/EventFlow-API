using EventFlow_API.Models;
using System.Text.Json.Serialization;

namespace EventFlow_API.Commands;

public class EventCommand
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public required string Location { get; set; }
    public int OrganizerId { get; set; }

    [JsonIgnore]
    public Organizer Organizer { get; set; }

    [JsonIgnore]
    public ICollection<Speaker> Speakers { get; set; }

    [JsonIgnore]
    public ICollection<Participant> Participants { get; set; }
}
