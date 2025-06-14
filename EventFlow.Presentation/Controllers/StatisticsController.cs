using EventFlow.Core.Models.DTOs;
using EventFlow.Core.Repository.Interfaces;

namespace EventFlow.Presentation.Controllers
{
    [ApiController]
    [Route("dashboard/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IParticipantRepository _participantRepository;

        public StatisticsController(
            IEventRepository eventRepository,
            IOrganizerRepository organizerRepository,
            ISpeakerRepository speakerRepository,
            IParticipantRepository participantRepository)
        {
            _eventRepository = eventRepository;
            _organizerRepository = organizerRepository;
            _speakerRepository = speakerRepository;
            _participantRepository = participantRepository;
        }

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
}