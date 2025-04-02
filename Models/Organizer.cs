using System.Collections.ObjectModel;

namespace EventFlow_API.Models;

public class Organizer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Event>? Events { get; set; } = new Collection<Event>();
}
