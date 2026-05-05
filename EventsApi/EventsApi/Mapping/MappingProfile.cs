using AutoMapper;
using EventsApi.DTOs;
using EventsApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Event: bidirectional mapping
        CreateMap<Event, EventDto>();
        CreateMap<CreateEventDto, Event>();

        // Organizer
        CreateMap<Organizer, OrganizerDto>();
        CreateMap<CreateOrganizerDto, Organizer>();

        // Ticket
        CreateMap<Ticket, TicketDto>();
        CreateMap<CreateTicketDto, Ticket>();
    }
}