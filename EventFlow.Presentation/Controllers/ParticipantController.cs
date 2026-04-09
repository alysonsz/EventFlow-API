using EventFlow.Application.Features.Participants.Commands.CreateParticipant;
using EventFlow.Application.Features.Participants.Commands.DeleteParticipant;
using EventFlow.Application.Features.Participants.Commands.UpdateParticipant;
using EventFlow.Application.Features.Participants.Queries.GetParticipantById;
using EventFlow.Application.Features.Participants.Queries.GetParticipantsByEventId;
using EventFlow.Core.Models;
using EventFlow.Presentation.Extensions;
using MediatR;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

[Route("participant")]
[ApiController]
public class ParticipantController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateParticipantCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
            return Ok(result.Value);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{eventId}/participant/{participantId}")]
    public async Task<IActionResult> RegisterParticipantAsync(int eventId, int participantId)
    {
        // TODO: Implementar RegisterParticipantToEventCommand
        await Task.CompletedTask;
        return Ok(new { message = "Participante vinculado ao evento com sucesso." });
    }

    [Authorize]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateParticipantCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await sender.Send(updatedCommand);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await sender.Send(new DeleteParticipantCommand(id));
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetParticipantByIdAsync(int id)
    {
        var result = await sender.Send(new GetParticipantByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpGet("{eventId}/all")]
    public async Task<IActionResult> GetAllParticipantsAsync(int eventId, [FromQuery] QueryParameters queryParameters)
    {
        var result = await sender.Send(new GetParticipantsByEventIdQuery(eventId, queryParameters));

        if (result.Items.Count == 0)
            return NotFound("Nenhum participante encontrado para este evento com os critérios fornecidos.");

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
