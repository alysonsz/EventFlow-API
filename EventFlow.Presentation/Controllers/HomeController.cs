namespace EventFlow.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Index()
    {
        return Ok(new { message = "ok" });
    }
}
