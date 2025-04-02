namespace EventFlow_API.Commands;

public class ParticipantCommand
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int EventId { get; set; }
}
