using AutoMapper;
using EventsApi.DTOs;
using EventsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public EventsController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET /api/events
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _db.Events
            .Include(e => e.Organizer)
            .ToListAsync();
        return Ok(_mapper.Map<List<EventDto>>(events));
    }

    // GET /api/events/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        return Ok(_mapper.Map<EventDto>(ev));
    }

    // POST /api/events
    [HttpPost]
    public async Task<IActionResult> Create(CreateEventDto dto)
    {
        var ev = _mapper.Map<Event>(dto);
        _db.Events.Add(ev);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById),
            new { id = ev.Id }, _mapper.Map<EventDto>(ev));
    }

    // PUT /api/events/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateEventDto dto)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        _mapper.Map(dto, ev);
        await _db.SaveChangesAsync();
        return Ok(_mapper.Map<EventDto>(ev));
    }

    // DELETE /api/events/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET /api/events/{eventId}/tickets
    [HttpGet("{eventId}/tickets")]
    public async Task<IActionResult> GetTickets(int eventId)
    {
        var exists = await _db.Events.AnyAsync(e => e.Id == eventId);
        if (!exists) return NotFound("Event not found");

        var tickets = await _db.Tickets
            .Where(t => t.EventId == eventId)
            .ToListAsync();
        return Ok(_mapper.Map<List<TicketDto>>(tickets));
    }

    // POST /api/events/{eventId}/tickets
    [HttpPost("{eventId}/tickets")]
    public async Task<IActionResult> CreateTicket(int eventId, CreateTicketDto dto)
    {
        var exists = await _db.Events.AnyAsync(e => e.Id == eventId);
        if (!exists) return NotFound("Event not found");

        var ticket = _mapper.Map<Ticket>(dto);
        ticket.EventId = eventId; // link to the event
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTickets),
            new { eventId }, _mapper.Map<TicketDto>(ticket));
    }

    // GET /api/events/{eventId}/organizer
    [HttpGet("{eventId}/organizer")]
    public async Task<IActionResult> GetOrganizer(int eventId)
    {
        var ev = await _db.Events
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev == null) return NotFound();
        return Ok(_mapper.Map<OrganizerDto>(ev.Organizer));
    }
}