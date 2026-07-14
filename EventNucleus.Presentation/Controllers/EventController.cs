using EventNucleus.Application.Features.Events.Commands.CreateEvent;
using EventNucleus.Application.Features.Events.Commands.DeleteEvent;
using EventNucleus.Application.Features.Events.Commands.UpdateEvent;
using EventNucleus.Application.Features.Events.Queries.GetAllEvents;
using EventNucleus.Application.Features.Events.Queries.GetEventById;
using EventNucleus.Core.Models;
using EventNucleus.Presentation.Extensions;
using System.Text.Json;

namespace EventNucleus.Presentation.Controllers;

[Route("event")]
[ApiController]
public class EventController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, null);
        
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { Id = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteEventCommand(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEventByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllEventsQuery(queryParameters), cancellationToken);

        if (result.Items.Count == 0)
            return NotFound();

        var metadata = new
        {
            result.TotalCount,
            result.PageSize,
            result.PageNumber,
            result.TotalPages,
            result.HasNextPage,
            result.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

        return Ok(result.Items);
    }
}

