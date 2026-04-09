using EventFlow.Application.DTOs;

namespace EventFlow.Application.Services;

public class RecommendationService(IParticipantRepository participantRepository, IMapper mapper) : IRecommendationService
{
    private readonly IParticipantRepository _participantRepository = participantRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<EventDTO>> GetRecommendedEventsAsync(int participantId)
    {
        var participantWithEvents = await _participantRepository.GetParticipantByIdAsync(participantId);
        var participantEventsIds = (participantWithEvents?.Events != null)
            ? participantWithEvents.Events.Select(e => e.Id).ToHashSet()
            : new HashSet<int>();

        if (participantEventsIds.Count == 0)
        {
            return Enumerable.Empty<EventDTO>();
        }

        var allParticipants = await _participantRepository.GetAllParticipantsWithEventsAsync();
        var similarParticipants = allParticipants
            .Where(p => p.Id != participantId && p.Events != null && p.Events.Count(e => participantEventsIds.Contains(e.Id)) > 0)
            .ToList();

        var recommendedEvents = similarParticipants
            .SelectMany(p => p.Events ?? Enumerable.Empty<Event>())
            .Where(e => !participantEventsIds.Contains(e.Id))
            .GroupBy(e => e.Id)
            .OrderByDescending(g => g.Count())
            .Select(g => g.First())
            .Take(10);

        return _mapper.Map<IEnumerable<EventDTO>>(recommendedEvents);
    }

    public async Task<IEnumerable<ParticipantDTO>> GetRecommendedConnectionsAsync(int participantId)
    {
        var participantWithEvents = await _participantRepository.GetParticipantByIdAsync(participantId);
        var participantEventsIds = (participantWithEvents?.Events != null)
            ? participantWithEvents.Events.Select(e => e.Id).ToHashSet()
            : new HashSet<int>();

        if (participantEventsIds.Count == 0)
        {
            return Enumerable.Empty<ParticipantDTO>();
        }

        var allParticipants = await _participantRepository.GetAllParticipantsWithEventsAsync();
        var recommendedParticipants = allParticipants
            .Where(p => p.Id != participantId)
            .Select(p => new
            {
                Participant = p,
                SharedEventsCount = (p.Events ?? Enumerable.Empty<Event>()).Count(e => participantEventsIds.Contains(e.Id))
            })
            .Where(x => x.SharedEventsCount > 0)
            .OrderByDescending(x => x.SharedEventsCount)
            .Select(x => x.Participant)
            .Take(10);

        return _mapper.Map<IEnumerable<ParticipantDTO>>(recommendedParticipants);
    }
}
