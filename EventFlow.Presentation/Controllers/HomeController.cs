namespace EventFlow.Presentation.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Home()
    {
        return Ok(new { message = "ok" });
    }
}
