using EventFlow.Application.DTOs;
using EventFlow.Core.Repository;

namespace EventFlow.Presentation.Controllers;

[ApiController]
[Route("dashboard/statistics")]
public class StatisticsController(IEventRepository eventRepository, IOrganizerRepository organizerRepository,
    ISpeakerRepository speakerRepository, IParticipantRepository participantRepository) : ControllerBase
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IOrganizerRepository _organizerRepository = organizerRepository;
    private readonly ISpeakerRepository _speakerRepository = speakerRepository;
    private readonly IParticipantRepository _participantRepository = participantRepository;

    [HttpGet("all")]
    public async Task<IActionResult> GetAllStats()
    {
        var stats = new DashboardStatsDTO
        {
            EventCount = await _eventRepository.EventCountAsync(),
            OrganizerCount = await _organizerRepository.OrganizerCountAsync(),
            SpeakerCount = await _speakerRepository.SpeakerCountAsync(),
            ParticipantCount = await _participantRepository.ParticipantCountAsync()
        };

        return Ok(stats);
    }
}