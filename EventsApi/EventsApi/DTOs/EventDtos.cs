namespace EventsApi.DTOs;

// What clients SEND to create an event
public class CreateEventDto
{
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = "";
    public int OrganizerId { get; set; }
}

public class CreateOrganizerDto
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
}

public class CreateTicketDto
{
    public string Type { get; set; } = "";
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }
}

    // What clients RECEIVE when reading events
public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = "";
    public string? BannerImageUrl { get; set; }
    public int OrganizerId { get; set; }
}