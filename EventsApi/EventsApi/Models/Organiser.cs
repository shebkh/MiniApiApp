using EventsApi.Models;

public class Organizer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}

public class Ticket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Type { get; set; } = "";
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }
    public Event? Event { get; set; }
}