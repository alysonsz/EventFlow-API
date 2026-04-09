namespace EventFlow.Presentation.Controllers;

[Route("recommendation")]
[ApiController]
[Authorize]
public class RecommendationController(IRecommendationService recommendationService) : ControllerBase
{
    [HttpGet("events/{participantId}")]
    public async Task<IActionResult> GetRecommendedEvents(int participantId)
    {
        var recommendations = await recommendationService.GetRecommendedEventsAsync(participantId);
        return Ok(recommendations);
    }

    [HttpGet("connections")]
    public async Task<IActionResult> GetRecommendedConnections(int participantId)
    {
        var connections = await recommendationService.GetRecommendedConnectionsAsync(participantId);
        return Ok(connections);
    }
}