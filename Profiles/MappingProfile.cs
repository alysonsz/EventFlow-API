using AutoMapper;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;

namespace EventFlow_API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDTO>()
            .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src => src.SpeakerEvents.Select(se => se.Speaker)));
        CreateMap<Organizer, OrganizerDTO>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events));
        CreateMap<Event, EventSummaryDTO>();
        CreateMap<Speaker, SpeakerDTO>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.SpeakerEvents.Select(se => se.Event)));
        CreateMap<Participant, ParticipantDTO>();
    }
}
