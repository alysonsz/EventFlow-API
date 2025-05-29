namespace EventFlow_API.Commands;

public class EventCommand
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public int OrganizerId { get; set; }
}
