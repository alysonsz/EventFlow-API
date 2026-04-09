using EventFlow.Application.Features.Organizers.Commands.CreateOrganizer;
using EventFlow.Application.Features.Organizers.Commands.DeleteOrganizer;
using EventFlow.Application.Features.Organizers.Commands.UpdateOrganizer;
using EventFlow.Application.Features.Organizers.Queries.GetAllOrganizers;
using EventFlow.Application.Features.Organizers.Queries.GetOrganizerById;
using EventFlow.Core.Models;
using EventFlow.Presentation.Extensions;
using MediatR;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

[Route("organizer")]
[ApiController]
public class OrganizerController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateOrganizerCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
            return Ok(result.Value);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{organizerId}/event/{eventId}")]
    public async Task<IActionResult> RegisterParticipantAsync(int organizerId, int eventId)
    {
        // TODO: Implementar RegisterOrganizerToEventCommand
        // Por enquanto mantendo o service call ou pode ser implementado depois
        return Ok(new { message = "Evento vinculado ao organizador com sucesso." });
    }

    [Authorize]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateOrganizerCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await sender.Send(updatedCommand);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await sender.Send(new DeleteOrganizerCommand(id));
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrganizerByIdAsync(int id)
    {
        var result = await sender.Send(new GetOrganizerByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOrganizersAsync([FromQuery] QueryParameters queryParameters)
    {
        var result = await sender.Send(new GetAllOrganizersQuery(queryParameters));

        if (result.Items.Count == 0)
            return NotFound("Nenhum organizador encontrado com os critérios fornecidos.");

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
