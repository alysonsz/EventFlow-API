namespace EventFlow_API.Models;

public class Speaker
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Biography { get; set; }
    public required string Email { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
}
