namespace EventNucleus.Application.Commands;

public class EventCommand
{
    public int Id { get; private set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public required string Location { get; set; }
    public int OrganizerId { get; set; }
}

