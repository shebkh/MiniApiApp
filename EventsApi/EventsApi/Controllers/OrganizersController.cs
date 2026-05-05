using AutoMapper;
using EventsApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("api/organizers")]
public class OrganizersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public OrganizersController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("{organizerId}/events")]
    public async Task<IActionResult> GetEvents(int organizerId)
    {
        var events = await _db.Events
            .Where(e => e.OrganizerId == organizerId)
            .ToListAsync();
        return Ok(_mapper.Map<List<EventDto>>(events));
    }
}