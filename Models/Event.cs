﻿using System.Collections.ObjectModel;

namespace EventFlow_API.Models;

public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public required string Location { get; set; }
    public Organizer Organizer { get; set; }
    public int OrganizerId { get; set; }
    public ICollection<Speaker> Speakers { get; set; } = new Collection<Speaker>();
    public ICollection<Participant> Participants { get; set; } = new Collection<Participant>();
}
